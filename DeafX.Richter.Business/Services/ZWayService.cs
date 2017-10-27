using DeafX.Richter.Business.Exceptions;
using DeafX.Richter.Business.Interfaces;
using DeafX.Richter.Business.Models.ZWay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using DeafX.Richter.Business.Models;
using System.Net.Http;
using DeafX.Richter.Common.Http;
using DeafX.Richter.Common.Http.Extensions;

namespace DeafX.Richter.Business.Services
{
    public class ZWayService : IDeviceService
    {
        #region Constants

        private const string ZAutomationUrl = "http://192.168.1.181:8083/ZAutomation/api/v1/";
        private const string Login = "*";
        private const string Password = "*";

        #endregion

        private HttpClient _httpClient;
        private CookieContainer _authenticationCookies;
        private Dictionary<string, IDevice> _deviceDictonary;
        private int _lastDeviceUpdate;
        private ILogger<ZWayService> _logger;
        private bool _successfullyInitated;
        private Task _updateDevicesTask;

        public ZWayService(HttpClient httpClient, ILogger<ZWayService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;

            _httpClient.BaseAddress = new Uri("http://192.168.1.181:8083/ZAutomation/api/v1/");
        }

        public async Task InitAsync()
        {
            try
            {
                _logger.LogWarning("Initiating ZWayService");
                _successfullyInitated = await AuthenticateServiceAsync();
                //_deviceDictonary = (await GetDeviceDataAsync(0)).devices.ToDictionary(d => d.id);
                //_updateDevicesTask = new Task(UpdateDevicesAsync);
                //_updateDevicesTask.Start();
                UpdateDevicesAsync();
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Failed to initiate ZWayService");
                _successfullyInitated = false;
            }
        }

        public async Task ToggleDeviceAsync(ZWayDevice device, bool toggleState)
        {
            var request = GenerateRequest(string.Format("devices/{0}/command/{1}", device.id, toggleState ? "on" : "off"));

            var result = await _httpClient.SendAsync(request);

            var deviceResponse = await result.Content.ReadAsJsonAsync<ZWayResponse<object>>();

            if (deviceResponse.code != 200)
            {
                throw new ZWayException("ZWay controller returned with error: " + deviceResponse.error);
            }
        }

        private async void UpdateDevicesAsync()
        {
            for (; ; )
            {
                var updatedDevicesData = await GetDeviceDataAsync(_lastDeviceUpdate);

                foreach (var device in updatedDevicesData.devices)
                {
                    var storedDevice = _deviceDictonary[device.id];
                    storedDevice.UpdateMetrics(device.metrics, updatedDevicesData.updateTime);
                }

                await Task.Delay(1000);
            }
        }

        private async Task<ZWayDeviceData> GetDeviceDataAsync(int since)
        {
            var request = GenerateRequest("devices");

            if (since != 0)
            {
                request.Properties.Add("since", since.ToString());
            }

            var result = await _httpClient.SendAsync(request);

            if(result.IsSuccessStatusCode)
            {
                _logger.LogError($"ZWay request for devices returned with status code {result.StatusCode}");
                return new ZWayDeviceData() { devices = new List<ZWayDevice>() };
            }

            var deviceResponse = await result.Content.ReadAsJsonAsync<ZWayResponse<ZWayDeviceData>>();

            if (deviceResponse.code != 200)
            {
                throw new ZWayException("ZWay controller returned with error: " + deviceResponse.error);
            }

            _lastDeviceUpdate = deviceResponse.data.updateTime;

            return deviceResponse.data;
        }

        private async Task<bool> AuthenticateServiceAsync()
        {
            var authParameters = new AuthenticationParameters()
            {
                login = Login,
                password = Password,
                remeberme = false
            };

            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(ZAutomationUrl + "login"),
                Content = new JsonContent(authParameters)
            };

            var result = await _httpClient.SendAsync(request);

            if(result.IsSuccessStatusCode)
            {
                _authenticationCookies = result.Headers.GetCookies(ZAutomationUrl);
                return true;
            }

            return false;
        }

        private HttpRequestMessage GenerateRequest(string path)
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(ZAutomationUrl + path),
            };

            request.Headers.Add("Cookie ", _authenticationCookies.GetCookieHeader(new Uri(ZAutomationUrl)));
            
            return request;
        }

        public async void PopulateDevice()
        {
            var zWayDevices = (await GetDeviceDataAsync(0)).devices;

            _deviceDictonary = zWayDevices.ToDictionary(d => d.id, d => new ZWaveToggleDevice(d))
        }

        public IDevice[] GetAllDevices()
        {
            throw new NotImplementedException();
        }

        public IDevice[] GetUpdatedDevices(int since)
        {
            throw new NotImplementedException();
        }

        public Task ToggleDeviceAsync(string deviceId, bool toggled)
        {
            throw new NotImplementedException();
        }
    }
}
