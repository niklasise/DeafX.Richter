using DeafX.Richter.Business.Interfaces;
using DeafX.Richter.Web.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeafX.Richter.Web.Hubs
{
    public class DevicesHub : Hub
    {

        private IDeviceService _deviceService;

        public DevicesHub(IDeviceService deviceService, IHubContext<DevicesHub> hubContext)
        {
            _deviceService = deviceService;

            _deviceService.OnDevicesUpdated += (o,e) => { OnDevicesUpdated(hubContext, e.UpdatedDevices); };
        }

        public override Task OnConnectedAsync()
        {
            var devices = _deviceService.GetAllDevices().Select(d => DeviceViewModel.FromDevice(d)).ToArray();

            Clients.Client(Context.ConnectionId).InvokeAsync("allDevices", new { devices = devices });

            return base.OnConnectedAsync();
        }

        public Task ToggleDevice(string deviceId, bool toggled)
        {
            return _deviceService.ToggleDeviceAsync(deviceId, toggled);
        }

        private void OnDevicesUpdated(IHubContext<DevicesHub> hubContext, IDevice[] devices)
        {
            var deviceModels = devices.Select(d => DeviceViewModel.FromDevice(d)).ToArray();

            hubContext.Clients.All.InvokeAsync("devicesUpdated", new { devices = deviceModels });
        }

    }
}
