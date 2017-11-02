﻿using DeafX.Richter.Business.Interfaces;
using DeafX.Richter.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeafX.Richter.Business.Services
{
    public class VirtualDeviceService : IDeviceService
    {
        private Dictionary<string, IDevice> _devices;

        public event OnDevicesUpdatedHandler OnDevicesUpdated;

        public VirtualDeviceService(/*IDeviceService subService, IViruta*/)
        {
            _devices = new Dictionary<string, IDevice>();


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
                throw new ArgumentException($"{nameof(VirtualDeviceService)} can only toggle devices of type {nameof(DeviceGroup)}");
            }

            var taskList = new List<Task>();

            foreach (var toggleDevice in deviceGrp.Devices)
            {
                taskList.Add(toggleDevice.ParentService.ToggleDeviceAsync(toggleDevice.Id, toggled));
            }

            await Task.WhenAll(taskList);

            deviceGrp.Toggled = toggled;

            OnDevicesUpdated.Invoke(this, new DevicesUpdatedEventArgs(new IDevice[] { deviceGrp }));
        }
    }
}