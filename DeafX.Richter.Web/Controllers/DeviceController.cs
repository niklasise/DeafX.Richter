using DeafX.Richter.Business.Interfaces;
using DeafX.Richter.Common.Extensions;
using DeafX.Richter.Web.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace DeafX.Richter.Web.Controllers
{
    [Route("api/[controller]")]
    public class DevicesController : Controller
    {
        private ILogger<DevicesController> _logger;
        private IDeviceService _deviceService;

        public DevicesController(ILogger<DevicesController> logger, IDeviceService deviceService)
        {
            _logger = logger;
            _deviceService = deviceService;
        }

        [HttpGet]
        public DeviceViewModelCollection GetAllDevices()
        {
            var timeStamp = DateTime.Now.ToUnixTimeStamp();

            return new DeviceViewModelCollection()
            {
                Devices = _deviceService.GetAllDevices().Select(d => DeviceViewModel.FromDevice(d)).ToArray(),
                LastUpdated = timeStamp
            };
        }

        [HttpGet("{since:int}")]
        public DeviceViewModelCollection GetUpdatedDevices(long since)
        {
            var timeStamp = DateTime.Now.ToUnixTimeStamp();

            return new DeviceViewModelCollection()
            {
                Devices = _deviceService.GetUpdatedDevices(DateTimeExtensions.FromUnixTimeStamp(since)).Select(d => DeviceViewModel.FromDevice(d)).ToArray(),
                LastUpdated = timeStamp
            };
        }

        [HttpPut("toggle/{deviceId}/{toggled:bool}")]
        public async void ToggleDevice(string deviceId, bool toggled)
        {
            await _deviceService.ToggleDeviceAsync(deviceId, toggled);
        }

        [HttpPut("setAutomated/{deviceId}/{automated:bool}")]
        public void SetAutomated(string deviceId, bool automated)
        {
            _deviceService.SetAutomated(deviceId, automated);
        }

        [HttpPut("setTimer/{deviceId}/{seconds:int}/{stateToSet:bool}")]
        public void SetTimer(string deviceId, int seconds, bool stateToSet)
        {
            _deviceService.SetTimer(deviceId, seconds, stateToSet);
        }

    }

}
