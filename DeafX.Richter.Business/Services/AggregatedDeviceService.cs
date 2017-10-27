using DeafX.Richter.Business.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace DeafX.Richter.Business.Services
{
    public class AggregatedDeviceService : IDeviceService
    {
        private class DeviceServiceMap
        {
            public IDevice Device { get; set; }
            public IDeviceService Service { get; set; }
        }

        private IDeviceService[] _services;
        private Dictionary<string, IDevice> _allDevices;

        public AggregatedDeviceService()
        {

        }

        public AggregatedDeviceService(params IDeviceService[] services)
        {
            _services = services;
        }

        public IDevice[] GetAllDevices()
        {
            if(_allDevices == null)
            {
                PopulateDevices();
            }

            return _allDevices.Values.ToArray();
        }

        public IDevice[] GetUpdatedDevices(int since)
        {
            return _allDevices.Values.Where(d => d.LastChanged > since).ToArray();
        }

        public void ToggleDevice(string deviceId, bool toggled)
        {
            if (!_allDevices.ContainsKey(deviceId))
            {
                throw new ArgumentException($"No paramater with id '{deviceId}' found");
            }

            var device = _allDevices[deviceId];

            device.ParentService.ToggleDevice(deviceId, toggled);
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
