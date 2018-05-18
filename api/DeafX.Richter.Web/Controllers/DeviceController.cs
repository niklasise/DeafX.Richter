using DeafX.Richter.Business.Interfaces;
using DeafX.Richter.Common.Extensions;
using DeafX.Richter.Web.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private IConfiguration _configuration;

        public DevicesController(ILogger<DevicesController> logger, IDeviceService deviceService, IConfiguration configuration)
        {
            _logger = logger;
            _deviceService = deviceService;
            _configuration = configuration;
        }

        [HttpGet]
        public DeviceViewModelCollection GetAllDevices([FromQuery]long since)
        {
            var timeStamp = DateTime.Now.ToUnixTimeStamp();

            if(since > 0)
            {
                return new DeviceViewModelCollection()
                {
                    Devices = _deviceService.GetUpdatedDevices(DateTimeExtensions.FromUnixTimeStamp(since))
                                    .Select(d => DeviceViewModel.FromDevice(d)).ToArray(),
                    LastUpdated = timeStamp
                };
            }
            else
            {
                var devices = _deviceService.GetAllDevices();
                var devicesToShow = _configuration.Get<AppConfiguration>().DevicesToShow;

                return new DeviceViewModelCollection()
                {
                    Devices = devicesToShow.Select(id => DeviceViewModel.FromDevice(devices.FirstOrDefault(d => d.Id == id))).ToArray(),
                    LastUpdated = timeStamp
                };
            }
     
        }

        [HttpGet("{id}")]
        public IActionResult GetDevice(string id)
        {
            var device = _deviceService.GetDevice(id);
            
            if(device == null)
            {
                return NotFound();
            }

            return new JsonResult(DeviceViewModel.FromDevice(device));
        }

        [HttpPut("toggle/{deviceId}/{toggled:bool}")]
        public async void ToggleDevice(string deviceId, bool toggled)
        {
            _logger.LogDebug("Toggling like a baws");

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

        [HttpPut("abortTimer/{deviceId}")]
        public void AbortTimer(string deviceId)
        {
            _deviceService.AbortTimer(deviceId);
        }

    }

}
