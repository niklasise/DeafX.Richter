using DeafX.Richter.Business.Interfaces;
using DeafX.Richter.Business.Models.Weather;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DeafX.Richter.Business.Services
{
    public class WeatherService : IDeviceService
    {
        #region Fields

        private HttpClient _httpClient;
        private ILogger<WeatherService> _logger;
        private WeatherConfiguration _configuration;

        private WeatherAirDevice _airTempDevice;
        private WeatherRoadDevice _roadTempDevice;
        private WeatherPercipitationDevice _precipitationDevice;
        private WeatherWindDevice _windDevice;

        #endregion

        private IDevice[] AllDevices
        {
            get
            {
                return new IDevice[]
                {
                    _airTempDevice,
                    _roadTempDevice,
                    _precipitationDevice,
                    _windDevice
                };
            }
        }

        #region Constructor

        public WeatherService(HttpClient httpClient, ILogger<WeatherService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        #endregion

        public async Task<bool> InitAsync(WeatherConfiguration configuration)
        {
            try
            {
                _logger.LogInformation("Initiating WeatherService");

                _configuration = configuration;

                GenerateDevices();
                var data = await RetrieveWeatherData();
                UpdateDeviceValues(data.Response.Result.First().WeatherStation.First());

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
            for (; ; )
            {
                await Task.Delay(_configuration.UpdateInterval);

                var data = await RetrieveWeatherData();
                UpdateDeviceValues(data.Response.Result.First().WeatherStation.First());
            }
        }

        private void GenerateDevices()
        {
            _airTempDevice = new WeatherAirDevice(
                id: "WeatherDevice_Air",
                title: "Lufttemperatur",
                parentService: this
            );

            _roadTempDevice = new WeatherRoadDevice(
                id: "WeatherDevice_Road",
                title: "Vägtemperatur",
                parentService:  this
            );

            _precipitationDevice = new WeatherPercipitationDevice(
                id: "WeatherDevice_Precipitation",
                title: "Nederbörd",
                parentService:  this
            );

            _windDevice = new WeatherWindDevice(
                id: "WeatherDevice_Wind",
                title: "Vind",
                parentService:  this
            );
        }


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

        public event OnDevicesUpdatedHandler OnDevicesUpdated;

        private void UpdateDeviceValues(WeatherStation weatherData)
        {
            _roadTempDevice.SetValue(weatherData.Measurement.Road.Temp);

            _airTempDevice.SetValue(
                value: weatherData.Measurement.Air.Temp,
                relativeHumidity: weatherData.Measurement.Air.RelativeHumidity
            );

            _precipitationDevice.SetValue(
                value: weatherData.Measurement.Precipitation.Amount,
                amountTextual: weatherData.Measurement.Precipitation.AmountName,
                type: weatherData.Measurement.Precipitation.Type
            );

            _windDevice.SetValue(
                value: weatherData.Measurement.Wind.Force,
                maxValue: weatherData.Measurement.Wind.ForceMax,
                direction: weatherData.Measurement.Wind.Direction,
                directionTextual: weatherData.Measurement.Wind.DirectionText
            );
        }

        private async Task<WeatherResponse> RetrieveWeatherData()
        {
            var request = GenerateWeatherRequest();

            var response = await _httpClient.SendAsync(request);

            if(!response.IsSuccessStatusCode)
            {
                throw new Exception($"WeatherData request returned with non successful status code: {response.StatusCode}");
            }

            try
            {
                var responseString = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<WeatherResponse>(responseString);
            }
            catch(Exception e)
            {
                throw new Exception("Error while reading weather data response", e);
            }
        }

        private HttpRequestMessage GenerateWeatherRequest()
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_configuration.ApiUrl),
            };

            var requestBody = "<REQUEST>" +
                                $"<LOGIN authenticationkey=\"{_configuration.ApiKey}\"/>" +
                                "<QUERY objecttype=\"WeatherStation\">" +
                                    "<FILTER>" +
                                        $"<EQ name=\"Id\" value=\"{_configuration.StationId}\" />" +
                                    "</FILTER>" +
                                "</QUERY>" +
                              "</REQUEST>";

            request.Content = new StringContent(requestBody, Encoding.UTF8, "text/xml");

            return request;
        }

        #region NotImplemented 

        public Task ToggleDeviceAsync(string deviceId, bool toggled)
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

        public void AbortTimer(string deviceId)
        {
            throw new NotImplementedException();
        }

        #endregion

    }

}