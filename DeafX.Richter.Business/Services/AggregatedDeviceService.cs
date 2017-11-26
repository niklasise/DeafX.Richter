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
        private IDateTimeProvider _dateTimeProvider;
        private Dictionary<string, IDevice> _allDevices;
        private Dictionary<IToggleDevice, ToggleAutomationRule> _toggleAutomationRules;

        public event OnDevicesUpdatedHandler OnDevicesUpdated;

        //public IEnumerable<ToggleAutomationRule> ToggleAutomationRules
        //{
        //    get
        //    {
        //        return _toggleAutomationRules.Values;
        //    }
        //}

        public AggregatedDeviceService(IDateTimeProvider dateTimeProvider, params IDeviceService[] services)
        {
            _dateTimeProvider = dateTimeProvider;
            _services = services;

            // Set child serivce OnDeviceUpdated to just forward to our own
            foreach(var service in _services)
            {
                service.OnDevicesUpdated += (sender, args) => { if (OnDevicesUpdated != null) { OnDevicesUpdated.Invoke(this, args); } };
            }

        }

        public AggregatedDeviceService(params IDeviceService[] services) : this(new DefaultDateTimeProvider(), services) { }

        public void Init(ToggleAutomationRuleConfiguration[] ruleConfigurations)
        {
            _toggleAutomationRules = new Dictionary<IToggleDevice, ToggleAutomationRule>();

            if (_allDevices == null)
            {
                PopulateDevices();
            }

            foreach (var ruleConfiguration in ruleConfigurations)
            {
                if(!_allDevices.ContainsKey(ruleConfiguration.DeviceToToggle))
                {
                    throw new ArgumentException($"Cannot find device with id '{ruleConfiguration.DeviceToToggle}'");
                }

                var deviceToToggle = _allDevices[ruleConfiguration.DeviceToToggle] as IToggleDevice;

                if (deviceToToggle == null)
                {
                    throw new ArgumentException($"Device with id '{ruleConfiguration.DeviceToToggle}' is not of type IToggleDevice");
                }

                if(_toggleAutomationRules.ContainsKey(deviceToToggle))
                {
                    throw new ArgumentException($"Device with id '{ruleConfiguration.DeviceToToggle}' cannot have multiple automation rules");
                }

                var condition = GetConditionFromConfiguration(ruleConfiguration.Condition);

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
            await  ToggleDeviceAsync(rule.ToggleDevice.Id, newState);
        }

        private IToggleAutomationCondition GetConditionFromConfiguration(IToggleAutomationConditionConfiguration configuration)
        {
            if(configuration is TimerConditionConfiguration)
            {
                return GetTimerConditionFromConfiguration(configuration as TimerConditionConfiguration);
            }
            else
            {
                return GetDeviceConditionFromConfiguration(configuration as DeviceConditionConfiguration);
            }
        }

        private TimerCondition GetTimerConditionFromConfiguration(TimerConditionConfiguration timerConfiguration)
        {
            var intervals = new List<TimerConditionInterval>();

            foreach (var interval in timerConfiguration.Intervals)
            {
                var additionalConditions = interval.AdditionalConditions?.Select(c => GetDeviceConditionFromConfiguration(c)).ToArray();

                intervals.Add(new TimerConditionInterval(
                    start: interval.Start,
                    end: interval.End,
                    additionalConditions: additionalConditions
                ));
            }

            return new TimerCondition(
                intervals: intervals.ToArray(),
                dateTimeProvider: _dateTimeProvider
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

        public async Task ToggleDeviceAsync(string deviceId, bool toggled)
        {
            if (!_allDevices.ContainsKey(deviceId))
            {
                throw new ArgumentException($"No paramater with id '{deviceId}' found");
            }

            var device = _allDevices[deviceId];

            await device.ParentService.ToggleDeviceAsync(deviceId, toggled);
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

    }
}
