using DeafX.Richter.Business.Exceptions;
using DeafX.Richter.Business.Interfaces;
using DeafX.Richter.Business.Models;
using DeafX.Richter.Business.Models.ZWay;
using DeafX.Richter.Common.Http;
using DeafX.Richter.Common.Http.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DeafX.Richter.Business.Services
{
    public class ZWayService : IDeviceService
    {

        #region Fields

        private HttpClient _httpClient;
        private string _authenticationCookie;
        private Dictionary<string, IDevice> _zWaveDeviceDictonary;
        private Dictionary<string, ZWayDevice> _zWayDeviceDictonary;
        private int _lastDeviceUpdate;
        private ILogger<ZWayService> _logger;
        private string _baseAdress;

        public event OnDevicesUpdatedHandler OnDevicesUpdated;

        #endregion

        #region Constructor

        public ZWayService(HttpClient httpClient, ILogger<ZWayService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        #endregion

        #region Public Methods

        public async Task<bool> InitAsync(ZWayConfiguration configuration)
        {
            try
            {
                _logger.LogInformation("Initiating ZWayService");

                _baseAdress = configuration.Adress.TrimEnd('/') + '/';

                await AuthenticateServiceAsync(configuration);
                await PopulateDevices(configuration.Devices);
                UpdateDevicesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Failed to initiate ZWayService");
                throw e;
            }
        }

        public async Task ToggleDeviceAsync(string deviceId, bool toggleState)
        {
            if(_zWaveDeviceDictonary.ContainsKey(deviceId))
            {
                throw new ArgumentException($"No device with id '{deviceId}' found");
            }

            var device = _zWaveDeviceDictonary[deviceId] as ZWavePowerPlugDevice;

            if(device == null)
            {
                throw new ArgumentException($"Device id '{deviceId}' is not a valid toggle device");
            }

            var request = GenerateRequest(string.Format("devices/{0}/command/{1}", device.InternalSwitchDevice.id, toggleState ? "on" : "off"));

            var result = await _httpClient.SendAsync(request);

            var deviceResponse = await result.Content.ReadAsJsonAsync<ZWayResponse<object>>();

            if (deviceResponse.code != 200)
            {
                throw new ZWayException("ZWay controller returned with error: " + deviceResponse.error);
            }
        }

        public async Task PopulateDevices(ZWayDeviceConfiguration[] deviceConfigurations)
        {
            _zWaveDeviceDictonary = new Dictionary<string, IDevice>();
            _zWayDeviceDictonary = (await GetDeviceDataAsync(0)).devices.ToDictionary(d => d.id);

            foreach (var configuration in deviceConfigurations)
            {
                if (!_zWayDeviceDictonary.ContainsKey(configuration.ZWayId))
                {
                    throw new ZWayDeviceConfigurationException($"No ZWayDevice found with id '{configuration.ZWayId}'");
                }

                if (configuration.ZWayPowerId != null && !_zWayDeviceDictonary.ContainsKey(configuration.ZWayPowerId))
                {
                    throw new ZWayDeviceConfigurationException($"No ZWayDevice found with id '{configuration.ZWayPowerId}'");
                }

                switch (configuration.Type)
                {
                    case "sensor":
                        _zWaveDeviceDictonary.Add(configuration.Id, new ZWaveSensorDevice(id: configuration.Id, zWayDevice: _zWayDeviceDictonary[configuration.ZWayId], parentService: this));
                        continue;
                    case "powerplug":
                        _zWaveDeviceDictonary.Add(configuration.Id, new ZWavePowerPlugDevice(id: configuration.Id, switchDevice: _zWayDeviceDictonary[configuration.ZWayId], powerDevice: _zWayDeviceDictonary[configuration.ZWayPowerId], parentService: this));
                        continue;
                    default:
                        throw new ZWayDeviceConfigurationException($"Unable to create device with type '{configuration.Type}'");
                }
            }

        }

        public IDevice[] GetAllDevices()
        {
            return _zWaveDeviceDictonary.Values.ToArray();
        }

        #endregion

        #region Private Methods

        private async void UpdateDevicesAsync()
        {
            for (; ; )
            {
                var updatedZWayDevices = await GetDeviceDataAsync(_lastDeviceUpdate);
                var updatedZWaveDevices = new List<IDevice>();

                foreach (var device in updatedZWayDevices.devices)
                {
                    if(!_zWayDeviceDictonary.ContainsKey(device.id))
                    {
                        _logger.LogWarning($"Cannot update ZWayDevice with id '{device.id}' since it is not found in device dictionary");
                    }

                    var storedDevice = _zWayDeviceDictonary[device.id];

                    // Only trigger update if device has a parent device
                    if(storedDevice.ParentDevice != null && storedDevice.UpdateMetrics(device.metrics) && !updatedZWaveDevices.Contains(storedDevice.ParentDevice))
                    {
                        updatedZWaveDevices.Add(storedDevice.ParentDevice);
                    }
                }

                if(updatedZWaveDevices.Count > 0 && OnDevicesUpdated != null)
                {
                    OnDevicesUpdated.Invoke(this, new DevicesUpdatedEventArgs(updatedZWaveDevices.ToArray()));
                }

                await Task.Delay(1000);
            }
        }

        private async Task<ZWayDeviceData> GetDeviceDataAsync(int since)
        {
            var request = GenerateRequest("devices");

            if (since != 0)
            {
                request.RequestUri = new Uri(request.RequestUri, $"?since={since}");
            }

            var result = await _httpClient.SendAsync(request);

            if(!result.IsSuccessStatusCode)
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

        private async Task<bool> AuthenticateServiceAsync(ZWayConfiguration configuration)
        {
            var authParameters = new AuthenticationParameters()
            {
                login = configuration.Username,
                password = configuration.Password,
                remeberme = false
            };

            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_baseAdress + "login"),
                Content = new JsonContent(authParameters)
            };

            var result = await _httpClient.SendAsync(request);

            if(result.IsSuccessStatusCode)
            {
                _authenticationCookie = result.Headers.GetValues("Set-Cookie")?.FirstOrDefault()?.Split(';')?.FirstOrDefault();
                return !string.IsNullOrEmpty(_authenticationCookie);
            }

            return false;
        }

        private HttpRequestMessage GenerateRequest(string path)
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(_baseAdress + path),
            };

            request.Headers.Add("Cookie", _authenticationCookie);
            
            return request;
        }

        #endregion

    }
}
