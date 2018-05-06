using DeafX.Richter.Business.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeafX.Richter.Business.Models;

namespace DeafX.Richter.Business.Services
{
    public class AggregatedDeviceService : IDeviceService
    {
        private IDeviceService[] _services;
        private Dictionary<string, IDevice> _allDevices;
        private Dictionary<IToggleDeviceInternal, ToggleAutomationRule> _toggleAutomationRules;

        public event OnDevicesUpdatedHandler OnDevicesUpdated;

        public AggregatedDeviceService(params IDeviceService[] services)
        {
            _services = services;

            // Set child serivce OnDeviceUpdated to just forward to our own
            foreach(var service in _services)
            {
                service.OnDevicesUpdated += (sender, args) => { if (OnDevicesUpdated != null) { OnDevicesUpdated.Invoke(this, args); } };
            }

        }

        public void Init(ToggleAutomationRuleConfiguration[] ruleConfigurations)
        {
            _toggleAutomationRules = new Dictionary<IToggleDeviceInternal, ToggleAutomationRule>();

            if (_allDevices == null)
            {
                PopulateDevices();
            }

            InitAutomationRules(ruleConfigurations);
            //InitToggleTimers();
        }

        private void InitToggleTimers(ToggleTimerConfiguration[] timerConfigurations)
        {
            foreach (var timerConfiguration in timerConfigurations)
            {
                if (!_allDevices.ContainsKey(timerConfiguration.DeviceToToggle))
                {
                    throw new ArgumentException($"Cannot find device with id '{timerConfiguration.DeviceToToggle}'");
                }

                var deviceToToggle = _allDevices[timerConfiguration.DeviceToToggle] as IToggleDeviceInternal;

                if (deviceToToggle == null)
                {
                    throw new ArgumentException($"Device with id '{timerConfiguration.DeviceToToggle}' is not of type IToggleDevice");
                }

                if (((IToggleDevice)deviceToToggle).Timer != null)
                {
                    throw new ArgumentException($"Device with id '{timerConfiguration.DeviceToToggle}' cannot have multiple timers");
                }

                if(timerConfiguration.ExpirationTime > DateTime.Now)
                {
                    var toggleTimer = new ToggleTimer(
                        deviceToToggle: deviceToToggle,
                        stateToToggle: timerConfiguration.StateToToggle,
                        expirationTime: timerConfiguration.ExpirationTime
                    );

                    toggleTimer.OnTimerExpired += OnToggleTimerExpired;
                    ((IToggleDeviceTimerSet)deviceToToggle).Timer = toggleTimer;

                    //_toggleTimers.Add(deviceToToggle, toggleTimer);
                }
            }

        }

        private void OnToggleTimerExpired(ToggleTimer sender)
        {
            sender.DeviceToToggle.ParentService.ToggleDeviceAsync(sender.DeviceToToggle.Id, sender.StateToToggle);

            sender.OnTimerExpired -= OnToggleTimerExpired;

            (sender.DeviceToToggle as IToggleDeviceTimerSet).Timer  = null;

            OnDevicesUpdated?.Invoke(this, new DevicesUpdatedEventArgs(new IDevice[] { sender.DeviceToToggle }));
            //_toggleTimers.Remove(sender.DeviceToToggle);
        }

        private void InitAutomationRules(ToggleAutomationRuleConfiguration[] ruleConfigurations)
        {
            foreach (var ruleConfiguration in ruleConfigurations)
            {
                if (!_allDevices.ContainsKey(ruleConfiguration.DeviceToToggle))
                {
                    throw new ArgumentException($"Cannot find device with id '{ruleConfiguration.DeviceToToggle}'");
                }

                var deviceToToggle = _allDevices[ruleConfiguration.DeviceToToggle] as IToggleDeviceInternal;

                if (deviceToToggle == null)
                {
                    throw new ArgumentException($"Device with id '{ruleConfiguration.DeviceToToggle}' is not of type IToggleDevice");
                }

                if (_toggleAutomationRules.ContainsKey(deviceToToggle))
                {
                    throw new ArgumentException($"Device with id '{ruleConfiguration.DeviceToToggle}' cannot have multiple automation rules");
                }

                var condition = GetConditionFromConfiguration(ruleConfiguration);

                var rule = new ToggleAutomationRule(
                        id: ruleConfiguration.Id,
                        toggleDevice: deviceToToggle,
                        condition: condition
                    );

                // Set toggle state for device according to the rule
                OnRuleStateChanged(rule, rule.State);

                rule.OnStateChanged += (sender, args) => OnRuleStateChanged(args.Rule, args.NewState);

                _toggleAutomationRules.Add(deviceToToggle, rule);
            }
        }

        private async void OnRuleStateChanged(ToggleAutomationRule rule, bool newState)
        {
            if (rule.ToggleDevice.Automated)
            {
                await ToggleDeviceAsync(rule.ToggleDevice.Id, newState);
            }
        }

        private IToggleAutomationCondition GetConditionFromConfiguration(ToggleAutomationRuleConfiguration configuration)
        {
            if(configuration.TimerCondition != null)
            {
                return GetTimerConditionFromConfiguration(configuration.TimerCondition);
            }
            else if (configuration.DeviceCondition != null)
            {
                return GetDeviceConditionFromConfiguration(configuration.DeviceCondition);
            }
            else
            {
                return null;
            }
        }

        private TimerCondition GetTimerConditionFromConfiguration(TimerConditionConfiguration timerConfiguration)
        {
            var intervals = new List<TimerConditionInterval>();

            foreach (var interval in timerConfiguration.Intervals)
            {
                var additionalConditions = interval.AdditionalConditions?.Select(c => GetDeviceConditionFromConfiguration(c)).ToArray();

                intervals.Add(new TimerConditionInterval(
                    start: TimeSpan.Parse(interval.Start),
                    end: TimeSpan.Parse(interval.End),
                    additionalConditions: additionalConditions
                ));
            }

            return new TimerCondition(
                intervals: intervals.ToArray()
            );
        }

        private DeviceCondition GetDeviceConditionFromConfiguration(DeviceConditionConfiguration deviceConfiguration)
        {
            if (!_allDevices.ContainsKey(deviceConfiguration.Device))
            {
                throw new ArgumentException($"Cannot find device with id '{deviceConfiguration.Device}'");
            }

            var device = _allDevices[deviceConfiguration.Device];

            return new DeviceCondition(
                device: device,
                compareValue: deviceConfiguration.CompareValue,
                compareOperator: deviceConfiguration.CompareOperator
            );
        }

        public IDevice[] GetAllDevices()
        {
            if(_allDevices == null)
            {
                PopulateDevices();
            }

            return _allDevices.Values.ToArray();
        }

        public IDevice GetDevice(string deviceId)
        {
            if (!_allDevices.ContainsKey(deviceId))
            {
                return null;
            }

            return _allDevices[deviceId];
        }

        public async Task ToggleDeviceAsync(string deviceId, bool toggled)
        {
            if (!_allDevices.ContainsKey(deviceId))
            {
                throw new ArgumentException($"No paramater with id '{deviceId}' found");
            }

            var device = _allDevices[deviceId];

            await device.ParentService.ToggleDeviceAsync(deviceId, toggled);
        }

        public void SetAutomated(string deviceId, bool automated)
        {
            if (!_allDevices.ContainsKey(deviceId))
            {
                throw new ArgumentException($"No paramater with id '{deviceId}' found");
            }

            var device = _allDevices[deviceId];

            device.ParentService.SetAutomated(deviceId, automated);
        }

        public void SetTimer(string deviceId, int seconds, bool stateToSet)
        {
            if (!_allDevices.ContainsKey(deviceId))
            {
                throw new ArgumentException($"No paramater with id '{deviceId}' found");
            }

            var device = _allDevices[deviceId] as IToggleDeviceInternal;

            if(device == null)
            {
                throw new ArgumentException($"Device must be of type {nameof(IToggleDeviceInternal)}");
            }

            var timer = new ToggleTimer(
                deviceToToggle: device,
                stateToToggle: stateToSet,
                expirationTime: DateTime.Now.AddSeconds(seconds)
            );

            timer.OnTimerExpired += OnToggleTimerExpired;

            ((IToggleDeviceTimerSet)device).Timer = timer;
            ((IDeviceLastChangedSet)device).LastChanged = DateTime.Now;

            OnDevicesUpdated?.Invoke(this, new DevicesUpdatedEventArgs(new IDevice[] { device }));
        }

        public void AbortTimer(string deviceId)
        {
            if (!_allDevices.ContainsKey(deviceId))
            {
                throw new ArgumentException($"No paramater with id '{deviceId}' found");
            }

            var device = _allDevices[deviceId] as IToggleDeviceInternal;

            if (device == null)
            {
                throw new ArgumentException($"Device must be of type {nameof(IToggleDeviceInternal)}");
            }

            if(((IToggleDevice)device).Timer == null)
            {
                return;
            }

            ((IToggleDevice)device).Timer.OnTimerExpired -= OnToggleTimerExpired;
            ((IToggleDeviceTimerSet)device).Timer = null;
            ((IDeviceLastChangedSet)device).LastChanged = DateTime.Now;
        }

        private void PopulateDevices()
        {
            _allDevices = new Dictionary<string, IDevice>();

            foreach(var service in _services)
            {
                foreach(var device in service.GetAllDevices())
                {
                    _allDevices.Add(device.Id, device);
                }
            }

        }

        public IDevice[] GetUpdatedDevices(DateTime since)
        {
            return _services.SelectMany(s => s.GetUpdatedDevices(since)).ToArray();
        }

    }
}
