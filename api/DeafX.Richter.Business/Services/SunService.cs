using DeafX.Richter.Business.Interfaces;
using DeafX.Richter.Business.Models.Sun;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DeafX.Richter.Business.Services
{
    public class SunService : IDeviceService
    {
        #region Fields

        private HttpClient _httpClient;
        private ILogger<SunService> _logger;
        private SunConfiguration _configuration;

        private SunDevice _sunDevice;

        #endregion

        private IDevice[] AllDevices
        {
            get
            {
                return new IDevice[]
                {
                    _sunDevice
                };
            }
        }

        #region Constructor

        public SunService(HttpClient httpClient, ILogger<SunService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        #endregion

        public async Task<bool> InitAsync(SunConfiguration configuration)
        {
            try
            {
                _logger.LogInformation("Initiating WeatherService");

                _configuration = configuration;

                _sunDevice = new SunDevice(
                    id: "Sun_Device",
                    title: "Soltider",
                    parentService: this
                );
                
                var data = await RetrieveSunData(DateTime.Now);

                UpdateDeviceValues(data);

                ScheduleUpdates();

                return true;
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Failed to initiate WeatherService!");
                throw e;
            }
        }

        private async void ScheduleUpdates()
        {
            for (;;)
            {
                var now = DateTime.Now;
                var timespanOfDay = new TimeSpan(
                    hours: now.Hour,
                    minutes: now.Minute,
                    seconds: now.Second
                );
                var timeUntilMidnight = TimeSpan.FromDays(1) - timespanOfDay;

                await Task.Delay(timeUntilMidnight);

                var data = await RetrieveSunData(DateTime.Now + TimeSpan.FromSeconds(1));

                UpdateDeviceValues(data);
            }
        }

        private void UpdateDeviceValues(SunResponse data)
        {
            _sunDevice.SetValues(
                    sunHours: data.results.day_length,
                    sunRise: data.results.sunrise,
                    sunSet: data.results.sunset
                );
        }

        private async Task<SunResponse> RetrieveSunData(DateTime dateTime)
        {
            var request = GenerateSunRequest(dateTime);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Sun data request returned with non successful status code: {response.StatusCode}");
            }

            try
            {
                var responseString = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<SunResponse>(responseString);
            }
            catch (Exception e)
            {
                throw new Exception("Error while reading weather data response", e);
            }
        }

        private HttpRequestMessage GenerateSunRequest(DateTime dateTime)
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_configuration.ApiUrl}/json?lat={_configuration.Latitude.ToString(CultureInfo.InvariantCulture)}&lng={_configuration.Longitude.ToString(CultureInfo.InvariantCulture)}&date={dateTime.ToString("yyyy-MM-dd")}&formatted=0"),
            };
            
            return request;
        }

        public event OnDevicesUpdatedHandler OnDevicesUpdated;

        public IDevice GetDevice(string id)
        {
            return AllDevices.FirstOrDefault(d => d.Id == id);
        }

        public IDevice[] GetAllDevices()
        {
            return AllDevices;
        }

        public IDevice[] GetUpdatedDevices(DateTime since)
        {
            return AllDevices.Where(d => d.LastChanged > since).ToArray();
        }

        public void AbortTimer(string deviceId)
        {
            throw new NotImplementedException();
        }

        public void SetAutomated(string deviceId, bool automated)
        {
            throw new NotImplementedException();
        }

        public void SetTimer(string deviceId, int seconds, bool stateToSet)
        {
            throw new NotImplementedException();
        }

        public Task ToggleDeviceAsync(string deviceId, bool toggled)
        {
            throw new NotImplementedException();
        }
    }
}
