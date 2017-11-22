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
        private List<ToggleTrigger> _triggers;

        public event OnDevicesUpdatedHandler OnDevicesUpdated;

        public IEnumerable<ToggleTrigger> ToggleTriggers
        {
            get
            {
                return _triggers;
            }
        }

        public AggregatedDeviceService(params IDeviceService[] services)
        {
            _services = services;

            // Set child serivce OnDeviceUpdated to just forward to our own
            foreach(var service in _services)
            {
                service.OnDevicesUpdated += (sender, args) => { if (OnDevicesUpdated != null) { OnDevicesUpdated.Invoke(this, args); } };
            }

        }

        public void Init(TriggerConfiguration[] triggerConfigurations)
        {
            _triggers = new List<ToggleTrigger>();

            if (_allDevices == null)
            {
                PopulateDevices();
            }

            foreach (var triggerConfiguration in triggerConfigurations)
            {
                if(!_allDevices.ContainsKey(triggerConfiguration.DeviceToToggle))
                {
                    throw new ArgumentException($"Cannot find device with id '{triggerConfiguration.DeviceToToggle}'");
                }

                var deviceToToggle = _allDevices[triggerConfiguration.DeviceToToggle] as IToggleDevice;

                if (deviceToToggle == null)
                {
                    throw new ArgumentException($"Device with id '{triggerConfiguration.DeviceToToggle}' is not of type IToggleDevice");
                }

                var conditions = GetConditionsFromConfiguration(triggerConfiguration.Conditions);

                var trigger = new ToggleTrigger(
                        id: triggerConfiguration.Id,
                        title: triggerConfiguration.Title,
                        stateToSet: triggerConfiguration.StateToSet,
                        deviceToToggle: deviceToToggle,
                        conditions: conditions
                    );

                // If triggered at initiation, invoke action
                if(trigger.Triggered)
                {
                    OnTriggerTriggered(trigger);
                }

                trigger.OnTriggered += OnTriggerTriggered;

                _triggers.Add(trigger);
            }
        }

        private async void OnTriggerTriggered(object sender)
        {
            var toggleTrigger = sender as ToggleTrigger;

            await  ToggleDeviceAsync(toggleTrigger.DeviceToToggle.Id, toggleTrigger.StateToSet);
        }

        private ITriggerCondition[] GetConditionsFromConfiguration(ITriggerConditionConfiguration[] conditions)
        {
            var triggerConditions = new List<ITriggerCondition>();

            foreach(var configuration in conditions)
            {
                if(configuration is TimeConditionConfiguration)
                {
                    var timeConfiguration = configuration as TimeConditionConfiguration;

                    triggerConditions.Add(new TimerCondition(
                        time: timeConfiguration.Time
                    ));
                }
                else
                {
                    var deviceConfiguration = configuration as DeviceConditionConfiguration;

                    if(!_allDevices.ContainsKey(deviceConfiguration.Device))
                    {
                        throw new ArgumentException($"Cannot find device with id '{deviceConfiguration.Device}'");
                    }

                    var device = _allDevices[deviceConfiguration.Device];

                    triggerConditions.Add(new DeviceCondition(
                        device: device,
                        compareValue: deviceConfiguration.CompareValue,
                        compareOperator: deviceConfiguration.CompareOperator
                    ));
                }
            }

            return triggerConditions.ToArray();
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
