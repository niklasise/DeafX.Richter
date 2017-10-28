using DeafX.Richter.Business.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeafX.Richter.Business.Services
{
    public class AggregatedDeviceService : IDeviceService
    {
        private IDeviceService[] _services;
        private Dictionary<string, IDevice> _allDevices;

        public event OnDevicesUpdatedHandler OnDevicesUpdated;

        public AggregatedDeviceService()
        {

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
