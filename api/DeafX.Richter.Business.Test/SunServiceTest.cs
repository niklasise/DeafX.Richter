using DeafX.Richter.Business.Models.Sun;
using DeafX.Richter.Business.Services;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DeafX.Richter.Business.Test
{
    [TestClass]
    public class SunServiceTest
    {
        private class MockData
        {
            public bool HasUpdates { get; set; }

            public SunConfiguration Configuration { get; set; } = new SunConfiguration()
            {
                ApiUrl = "http://test.api",
                Latitude = 1.12345,
                Longitude = 2.3456
            };
        }


        [TestMethod]
        public async Task Init()
        {
            var data = new MockData();
            var container = GetMockContainer(data);

            var result = await container.Service.InitAsync(data.Configuration);

            Assert.IsTrue(result);

            var sunDevice = container.Service.GetDevice("Sun_Device") as SunDevice;
            
            Assert.AreEqual(62058, sunDevice.Value);

            Assert.AreEqual(DateTime.Parse("2018-05-18T02:08:12+00:00"), sunDevice.SunRise);
            Assert.AreEqual(DateTime.Parse("2018-05-18T19:22:30+00:00"), sunDevice.SunSet);
        }

        //[TestMethod]
        //public async Task NoUpdates()
        //{
        //    var data = new MockData();
        //    var container = GetMockContainer(data);

        //    await container.Service.InitAsync(data.Configuration);

        //    var dateTime = DateTime.Now;

        //    await Task.Delay(1500);

        //    var result = container.Service.GetUpdatedDevices(dateTime);

        //    Assert.AreEqual(0, result.Length);
        //}

        //[TestMethod]
        //public async Task Updates()
        //{
        //    var data = new MockData()
        //    {
        //        HasUpdates = true
        //    };
        //    var container = GetMockContainer(data);

        //    await container.Service.InitAsync(data.Configuration);

        //    var dateTime = DateTime.Now;

        //    await Task.Delay(1500);

        //    var result = container.Service.GetUpdatedDevices(dateTime);

        //    Assert.AreEqual(4, result.Length);

        //    var airDevice = result.First(d => d.Id == "WeatherDevice_Air") as WeatherAirDevice;
        //    var roadDevice = result.First(d => d.Id == "WeatherDevice_Road") as WeatherRoadDevice;
        //    var precipitationDevice = result.First(d => d.Id == "WeatherDevice_Precipitation") as WeatherPercipitationDevice;
        //    var windDevice = result.First(d => d.Id == "WeatherDevice_Wind") as WeatherWindDevice;

        //    Assert.AreEqual(21.7, airDevice.Value);
        //    Assert.AreEqual(40.5, airDevice.RealtiveHumidity);

        //    Assert.AreEqual(33.1, roadDevice.Value);

        //    Assert.AreEqual(2.2, precipitationDevice.Value);
        //    Assert.AreEqual("Mkt Regn", precipitationDevice.AmountTextual);
        //    Assert.AreEqual("Regn", precipitationDevice.Type);

        //    Assert.AreEqual(1.1, windDevice.Value);
        //    Assert.AreEqual(3.4, windDevice.MaxValue);
        //    Assert.AreEqual(180, windDevice.Direction);
        //    Assert.AreEqual("Syd", windDevice.DirectionTextual);
        //}

        private MockContainer GetMockContainer(MockData data)
        {
            var mockContainer = new MockContainer()
            {
                Logger = new Mock<ILogger<SunService>>(),
                Http = GetMockHttpMessageHandler(data)
            };

            mockContainer.Service = new SunService(new HttpClient(mockContainer.Http), mockContainer.Logger.Object);

            return mockContainer;
        }

        private MockHttpMessageHandler GetMockHttpMessageHandler(MockData data)
        {
            var mock = new MockHttpMessageHandler();

            var apiUrl = $"{data.Configuration.ApiUrl}/json?lat={data.Configuration.Latitude.ToString(CultureInfo.InvariantCulture)}&lng={data.Configuration.Longitude.ToString(CultureInfo.InvariantCulture)}&date={DateTime.Now.ToString("yyyy-MM-dd")}&formatted=0";

            mock.Expect(HttpMethod.Get, apiUrl).Respond("application/json", "{\"results\":{\"sunrise\":\"2018-05-18T02:08:12+00:00\",\"sunset\":\"2018-05-18T19:22:30+00:00\",\"solar_noon\":\"2018-05-18T10:45:21+00:00\",\"day_length\":62058,\"civil_twilight_begin\":\"2018-05-18T01:05:06+00:00\",\"civil_twilight_end\":\"2018-05-18T20:25:36+00:00\",\"nautical_twilight_begin\":\"1970-01-01T00:00:01+00:00\",\"nautical_twilight_end\":\"1970-01-01T00:00:01+00:00\",\"astronomical_twilight_begin\":\"1970-01-01T00:00:01+00:00\",\"astronomical_twilight_end\":\"1970-01-01T00:00:01+00:00\"},\"status\":\"OK\"}");

            if (data.HasUpdates)
            {
                mock.Expect(HttpMethod.Get, apiUrl).Respond("application/json", "{\"results\":{\"sunrise\":\"2018-05-18T02:08:12+00:00\",\"sunset\":\"2018-05-18T19:22:30+00:00\",\"solar_noon\":\"2018-05-18T10:45:21+00:00\",\"day_length\":62058,\"civil_twilight_begin\":\"2018-05-18T01:05:06+00:00\",\"civil_twilight_end\":\"2018-05-18T20:25:36+00:00\",\"nautical_twilight_begin\":\"1970-01-01T00:00:01+00:00\",\"nautical_twilight_end\":\"1970-01-01T00:00:01+00:00\",\"astronomical_twilight_begin\":\"1970-01-01T00:00:01+00:00\",\"astronomical_twilight_end\":\"1970-01-01T00:00:01+00:00\"},\"status\":\"OK\"}");
            }

            mock.When(HttpMethod.Get, apiUrl).Respond("application/json", "{\"results\":{\"sunrise\":\"2018-05-18T02:08:12+00:00\",\"sunset\":\"2018-05-18T19:22:30+00:00\",\"solar_noon\":\"2018-05-18T10:45:21+00:00\",\"day_length\":62058,\"civil_twilight_begin\":\"2018-05-18T01:05:06+00:00\",\"civil_twilight_end\":\"2018-05-18T20:25:36+00:00\",\"nautical_twilight_begin\":\"1970-01-01T00:00:01+00:00\",\"nautical_twilight_end\":\"1970-01-01T00:00:01+00:00\",\"astronomical_twilight_begin\":\"1970-01-01T00:00:01+00:00\",\"astronomical_twilight_end\":\"1970-01-01T00:00:01+00:00\"},\"status\":\"OK\"}");

            return mock;
        }

        private class MockContainer
        {
            public Mock<ILogger<SunService>> Logger { get; set; }
            public MockHttpMessageHandler Http { get; set; }

            public SunService Service { get; set; }
        }
    }
}
