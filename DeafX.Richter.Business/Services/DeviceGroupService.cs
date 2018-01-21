using DeafX.Richter.Business.Interfaces;
using DeafX.Richter.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeafX.Richter.Business.Services
{
    public class DeviceGroupService : IDeviceService
    {
        private Dictionary<string, DeviceGroup> _devices;
        private IDeviceService[] _subServices;

        public event OnDevicesUpdatedHandler OnDevicesUpdated;

        public DeviceGroupService(params IDeviceService[] services)
        {
            _devices = new Dictionary<string, DeviceGroup>();
            _subServices = services;
        }

        public void Init(DeviceGroupConfiguration[] deviceConfigurations)
        {
            var allDevices = _subServices.SelectMany(s => s.GetAllDevices()).ToDictionary(d => d.Id);

            foreach (var deviceConfiguration in deviceConfigurations)
            {
                var devices = new List<IToggleDevice>();

                foreach(var id in deviceConfiguration.Devices)
                {
                    allDevices.TryGetValue(id, out IDevice device);

                    if(device == null || !(device is IToggleDevice))
                    {
                        throw new ArgumentException($"Device with id '{id}' does not exist or is not of type IToggleDevice");
                    }

                    devices.Add(device as IToggleDevice);
                }

                var deviceGrp = new DeviceGroup(
                    id: deviceConfiguration.Id,
                    title: deviceConfiguration.Title,
                    automated: deviceConfiguration.Automated,
                    devices: devices.ToArray(),
                    parentService: this
                    );

                deviceGrp.LastChanged = DateTime.Now;

                _devices.Add(deviceConfiguration.Id, deviceGrp);
            }
        }

        public IDevice[] GetAllDevices()
        {
            return _devices.Values.ToArray();
        }

        public async Task ToggleDeviceAsync(string deviceId, bool toggled)
        {
            if(!_devices.ContainsKey(deviceId))
            {
                throw new ArgumentException($"No device found with id '{deviceId}'");
            }

            var deviceGrp = _devices[deviceId] as DeviceGroup;

            if(deviceGrp == null)
            {
                throw new ArgumentException($"{nameof(DeviceGroupService)} can only toggle devices of type {nameof(DeviceGroup)}");
            }

            var taskList = new List<Task>();

            foreach (var toggleDevice in deviceGrp.Devices)
            {
                taskList.Add(toggleDevice.ParentService.ToggleDeviceAsync(toggleDevice.Id, toggled));
            }

            await Task.WhenAll(taskList);

            deviceGrp.Toggled = toggled;
            deviceGrp.LastChanged = DateTime.Now;
            
            OnDevicesUpdated?.Invoke(this, new DevicesUpdatedEventArgs(new IDevice[] { deviceGrp }));
        }

        public void SetAutomated(string deviceId, bool automated)
        {
            if(!_devices.ContainsKey(deviceId))
            {
                throw new ArgumentException($"No device with id '{deviceId}'");
            }

            var device = _devices[deviceId];

            device.Automated = automated;

            OnDevicesUpdated?.Invoke(this, new DevicesUpdatedEventArgs(new IDevice[] { device }));
        }

        public void SetTimer(string deviceId, int seconds, bool stateToSet)
        {
            throw new NotImplementedException();
        }

        public void AbortTimer(string deviceId)
        {
            throw new NotImplementedException();
        }

        public IDevice[] GetUpdatedDevices(DateTime since)
        {
            return _devices.Values.Where(d => d.LastChanged > since).ToArray();
        }

    }
}
