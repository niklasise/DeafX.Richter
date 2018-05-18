using DeafX.Richter.Business.Models.Weather;
using DeafX.Richter.Business.Services;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DeafX.Richter.Business.Test
{
    [TestClass]
    public class WeatherServiceTest
    {
        private class MockData
        {
            public bool HasUpdates { get; set; }

            public WeatherConfiguration Configuration { get; set; } = new WeatherConfiguration()
            {
                ApiKey = "abc123",
                ApiUrl = "http://test.api/json",
                StationId = "def456",
                UpdateInterval = 1000
            };
        }


        [TestMethod]
        public async Task Init()
        {
            var data = new MockData();
            var container = GetMockContainer(data);

            var result = await container.Service.InitAsync(data.Configuration);

            Assert.IsTrue(result);

            var airDevice = container.Service.GetDevice("Weather_Air_Device") as WeatherAirDevice;
            var roadDevice = container.Service.GetDevice("Weather_Road_Device") as WeatherRoadDevice;
            var precipitationDevice = container.Service.GetDevice("Weather_Precipitation_Device") as WeatherPercipitationDevice;
            var windDevice = container.Service.GetDevice("Weather_Wind_Device") as WeatherWindDevice;

            Assert.AreEqual(22.7, airDevice.Value);
            Assert.AreEqual(35.5, airDevice.RealtiveHumidity);

            Assert.AreEqual(34.1, roadDevice.Value);

            Assert.AreEqual(0.0, precipitationDevice.Value);
            Assert.AreEqual("Ingen nederbörd", precipitationDevice.AmountTextual);
            Assert.AreEqual("Ingen nederbörd", precipitationDevice.Type);

            Assert.AreEqual(3.1, windDevice.Value);
            Assert.AreEqual(7.2, windDevice.MaxValue);
            Assert.AreEqual(135, windDevice.Direction);
            Assert.AreEqual("Sydöst", windDevice.DirectionTextual);
        }

        [TestMethod]
        public async Task NoUpdates()
        {
            var data = new MockData();
            var container = GetMockContainer(data);

            await container.Service.InitAsync(data.Configuration);

            var dateTime = DateTime.Now;

            await Task.Delay(1500);

            var result = container.Service.GetUpdatedDevices(dateTime);

            Assert.AreEqual(0, result.Length);
        }

        [TestMethod]
        public async Task Updates()
        {
            var data = new MockData()
            {
                HasUpdates = true
            };
            var container = GetMockContainer(data);

            await container.Service.InitAsync(data.Configuration);

            var dateTime = DateTime.Now;

            await Task.Delay(1500);

            var result = container.Service.GetUpdatedDevices(dateTime);

            Assert.AreEqual(4, result.Length);

            var airDevice = result.First(d => d.Id == "Weather_Air_Device") as WeatherAirDevice;
            var roadDevice = result.First(d => d.Id == "Weather_Road_Device") as WeatherRoadDevice;
            var precipitationDevice = result.First(d => d.Id == "Weather_Precipitation_Device") as WeatherPercipitationDevice;
            var windDevice = result.First(d => d.Id == "Weather_Wind_Device") as WeatherWindDevice;

            Assert.AreEqual(21.7, airDevice.Value);
            Assert.AreEqual(40.5, airDevice.RealtiveHumidity);

            Assert.AreEqual(33.1, roadDevice.Value);

            Assert.AreEqual(2.2, precipitationDevice.Value);
            Assert.AreEqual("Mkt Regn", precipitationDevice.AmountTextual);
            Assert.AreEqual("Regn", precipitationDevice.Type);

            Assert.AreEqual(1.1, windDevice.Value);
            Assert.AreEqual(3.4, windDevice.MaxValue);
            Assert.AreEqual(180, windDevice.Direction);
            Assert.AreEqual("Syd", windDevice.DirectionTextual);
        }

        private MockContainer GetMockContainer(MockData data)
        {
            var mockContainer = new MockContainer()
            {
                Logger = new Mock<ILogger<WeatherService>>(),
                Http = GetMockHttpMessageHandler(data)
            };

            mockContainer.Service = new WeatherService(new HttpClient(mockContainer.Http), mockContainer.Logger.Object);

            return mockContainer;
        }

        private MockHttpMessageHandler GetMockHttpMessageHandler(MockData data)
        {
            var mock = new MockHttpMessageHandler();

            mock.Expect(HttpMethod.Post, data.Configuration.ApiUrl).Respond("application/json", "{\"RESPONSE\":{\"RESULT\":[{\"WeatherStation\":[{\"Active\":true,\"CountyNo\":[2,1], \"Geometry\":{\"SWEREF99TM\":\"POINT (657232.31 6574253.87)\",\"WGS84\":\"POINT (17.759519577026367 59.277908325195313)\"},\"IconId\":\"weatherStation\",\"Id\":\"SE_STA_VVIS241\", \"Measurement\":{\"MeasureTime\":\"2018-05-07T12:10:03\", \"Precipitation\":{\"AmountName\":\"Ingen nederb\u00F6rd\",\"Type\":\"Ingen nederb\u00F6rd\",\"TypeIconId\":\"precipitationNoPrecipitation\"}, \"Road\":{\"Temp\":34.1,\"TempIconId\":\"tempAirRoad\"}, \"Air\":{\"Temp\":22.7,\"TempIconId\":\"tempAirRoad\",\"RelativeHumidity\":35.5}, \"Wind\":{\"Direction\":135,\"DirectionIconId\":\"windSE\",\"DirectionText\":\"Syd\u00F6st\",\"Force\":3.1,\"ForceMax\":7.2}},\"MeasurementHistory\":[{\"MeasureTime\":\"2018-05-07T12:10:03\", \"Precipitation\":{\"AmountName\":\"Ingen nederb\u00F6rd\",\"Type\":\"Ingen nederb\u00F6rd\",\"TypeIconId\":\"precipitationNoPrecipitation\"}, \"Road\":{\"Temp\":34.1,\"TempIconId\":\"tempAirRoad\"}, \"Air\":{\"Temp\":22.7,\"TempIconId\":\"tempAirRoad\",\"RelativeHumidity\":35.5}, \"Wind\":{\"Direction\":135,\"DirectionIconId\":\"windSE\",\"DirectionText\":\"Syd\u00F6st\",\"Force\":3.1,\"ForceMax\":7.2}},{\"MeasureTime\":\"2018-05-07T12:00:03\", \"Precipitation\":{\"AmountName\":\"Ingen nederb\u00F6rd\",\"Type\":\"Ingen nederb\u00F6rd\",\"TypeIconId\":\"precipitationNoPrecipitation\"}, \"Road\":{\"Temp\":34,\"TempIconId\":\"tempAirRoad\"}, \"Air\":{\"Temp\":22.8,\"TempIconId\":\"tempAirRoad\",\"RelativeHumidity\":36.8}, \"Wind\":{\"Direction\":135,\"DirectionIconId\":\"windSE\",\"DirectionText\":\"Syd\u00F6st\",\"Force\":2.9,\"ForceMax\":7.2}},{\"MeasureTime\":\"2018-05-07T11:50:04\", \"Precipitation\":{\"AmountName\":\"Ingen nederb\u00F6rd\",\"Type\":\"Ingen nederb\u00F6rd\",\"TypeIconId\":\"precipitationNoPrecipitation\"}, \"Road\":{\"Temp\":34,\"TempIconId\":\"tempAirRoad\"}, \"Air\":{\"Temp\":22.6,\"TempIconId\":\"tempAirRoad\",\"RelativeHumidity\":35.5}, \"Wind\":{\"Direction\":135,\"DirectionIconId\":\"windSE\",\"DirectionText\":\"Syd\u00F6st\",\"Force\":2.8,\"ForceMax\":7.2}},{\"MeasureTime\":\"2018-05-07T11:40:03\", \"Precipitation\":{\"AmountName\":\"Ingen nederb\u00F6rd\",\"Type\":\"Ingen nederb\u00F6rd\",\"TypeIconId\":\"precipitationNoPrecipitation\"}, \"Road\":{\"Temp\":33.5,\"TempIconId\":\"tempAirRoad\"}, \"Air\":{\"Temp\":22.7,\"TempIconId\":\"tempAirRoad\",\"RelativeHumidity\":37.9}, \"Wind\":{\"Direction\":135,\"DirectionIconId\":\"windSE\",\"DirectionText\":\"Syd\u00F6st\",\"Force\":2.6,\"ForceMax\":6.2}},{\"MeasureTime\":\"2018-05-07T11:30:00\", \"Precipitation\":{\"AmountName\":\"Ingen nederb\u00F6rd\",\"Type\":\"Ingen nederb\u00F6rd\",\"TypeIconId\":\"precipitationNoPrecipitation\"}, \"Road\":{\"Temp\":33,\"TempIconId\":\"tempAirRoad\"}, \"Air\":{\"Temp\":22.6,\"TempIconId\":\"tempAirRoad\",\"RelativeHumidity\":35.5}, \"Wind\":{\"Direction\":135,\"DirectionIconId\":\"windSE\",\"DirectionText\":\"Syd\u00F6st\",\"Force\":2.5,\"ForceMax\":6.2}}],\"ModifiedTime\":\"2018-05-07T10:15:16.603Z\",\"Name\":\"Eker\u00F6\",\"RoadNumberNumeric\":816}]}]}}");

            if (data.HasUpdates)
            {
                mock.Expect(HttpMethod.Post, data.Configuration.ApiUrl).Respond("application/json", "{\"RESPONSE\":{\"RESULT\":[{\"WeatherStation\":[{\"Active\":true,\"CountyNo\":[2,1],\"Geometry\":{\"SWEREF99TM\":\"POINT (657232.31 6574253.87)\",\"WGS84\":\"POINT (17.759519577026367 59.277908325195313)\"},\"IconId\":\"weatherStation\",\"Id\":\"SE_STA_VVIS241\",\"Measurement\":{\"MeasureTime\":\"2018-05-07T12:10:03\",\"Precipitation\":{\"AmountName\":\"Mkt Regn\",\"Amount\":2.2,\"Type\":\"Regn\",\"TypeIconId\":\"precipitationNoPrecipitation\"},\"Road\":{\"Temp\":33.1,\"TempIconId\":\"tempAirRoad\"},\"Air\":{\"Temp\":21.7,\"TempIconId\":\"tempAirRoad\",\"RelativeHumidity\":40.5},\"Wind\":{\"Direction\":180,\"DirectionIconId\":\"windS\",\"DirectionText\":\"Syd\",\"Force\":1.1,\"ForceMax\":3.4}},\"MeasurementHistory\":[{\"MeasureTime\":\"2018-05-07T12:10:03\",\"Precipitation\":{\"AmountName\":\"Ingen nederb\u00F6rd\",\"Type\":\"Ingen nederb\u00F6rd\",\"TypeIconId\":\"precipitationNoPrecipitation\"},\"Road\":{\"Temp\":34.1,\"TempIconId\":\"tempAirRoad\"},\"Air\":{\"Temp\":22.7,\"TempIconId\":\"tempAirRoad\",\"RelativeHumidity\":35.5},\"Wind\":{\"Direction\":135,\"DirectionIconId\":\"windSE\",\"DirectionText\":\"Syd\u00F6st\",\"Force\":3.1,\"ForceMax\":7.2}},{\"MeasureTime\":\"2018-05-07T12:00:03\",\"Precipitation\":{\"AmountName\":\"Ingen nederb\u00F6rd\",\"Type\":\"Ingen nederb\u00F6rd\",\"TypeIconId\":\"precipitationNoPrecipitation\"},\"Road\":{\"Temp\":34,\"TempIconId\":\"tempAirRoad\"},\"Air\":{\"Temp\":22.8,\"TempIconId\":\"tempAirRoad\",\"RelativeHumidity\":36.8},\"Wind\":{\"Direction\":135,\"DirectionIconId\":\"windSE\",\"DirectionText\":\"Syd\u00F6st\",\"Force\":2.9,\"ForceMax\":7.2}},{\"MeasureTime\":\"2018-05-07T11:50:04\",\"Precipitation\":{\"AmountName\":\"Ingen nederb\u00F6rd\",\"Type\":\"Ingen nederb\u00F6rd\",\"TypeIconId\":\"precipitationNoPrecipitation\"},\"Road\":{\"Temp\":34,\"TempIconId\":\"tempAirRoad\"},\"Air\":{\"Temp\":22.6,\"TempIconId\":\"tempAirRoad\",\"RelativeHumidity\":35.5},\"Wind\":{\"Direction\":135,\"DirectionIconId\":\"windSE\",\"DirectionText\":\"Syd\u00F6st\",\"Force\":2.8,\"ForceMax\":7.2}},{\"MeasureTime\":\"2018-05-07T11:40:03\",\"Precipitation\":{\"AmountName\":\"Ingen nederb\u00F6rd\",\"Type\":\"Ingen nederb\u00F6rd\",\"TypeIconId\":\"precipitationNoPrecipitation\"},\"Road\":{\"Temp\":33.5,\"TempIconId\":\"tempAirRoad\"},\"Air\":{\"Temp\":22.7,\"TempIconId\":\"tempAirRoad\",\"RelativeHumidity\":37.9},\"Wind\":{\"Direction\":135,\"DirectionIconId\":\"windSE\",\"DirectionText\":\"Syd\u00F6st\",\"Force\":2.6,\"ForceMax\":6.2}},{\"MeasureTime\":\"2018-05-07T11:30:00\",\"Precipitation\":{\"AmountName\":\"Ingen nederb\u00F6rd\",\"Type\":\"Ingen nederb\u00F6rd\",\"TypeIconId\":\"precipitationNoPrecipitation\"},\"Road\":{\"Temp\":33,\"TempIconId\":\"tempAirRoad\"},\"Air\":{\"Temp\":22.6,\"TempIconId\":\"tempAirRoad\",\"RelativeHumidity\":35.5},\"Wind\":{\"Direction\":135,\"DirectionIconId\":\"windSE\",\"DirectionText\":\"Syd\u00F6st\",\"Force\":2.5,\"ForceMax\":6.2}}],\"ModifiedTime\":\"2018-05-07T10:15:16.603Z\",\"Name\":\"Eker\u00F6\",\"RoadNumberNumeric\":816}]}]}}");
            }

            mock.When(HttpMethod.Post, data.Configuration.ApiUrl).Respond("application/json", "{\"RESPONSE\":{\"RESULT\":[{\"WeatherStation\":[{\"Active\":true,\"CountyNo\":[2,1], \"Geometry\":{\"SWEREF99TM\":\"POINT (657232.31 6574253.87)\",\"WGS84\":\"POINT (17.759519577026367 59.277908325195313)\"},\"IconId\":\"weatherStation\",\"Id\":\"SE_STA_VVIS241\", \"Measurement\":{\"MeasureTime\":\"2018-05-07T12:10:03\", \"Precipitation\":{\"AmountName\":\"Ingen nederb\u00F6rd\",\"Type\":\"Ingen nederb\u00F6rd\",\"TypeIconId\":\"precipitationNoPrecipitation\"}, \"Road\":{\"Temp\":34.1,\"TempIconId\":\"tempAirRoad\"}, \"Air\":{\"Temp\":22.7,\"TempIconId\":\"tempAirRoad\",\"RelativeHumidity\":35.5}, \"Wind\":{\"Direction\":135,\"DirectionIconId\":\"windSE\",\"DirectionText\":\"Syd\u00F6st\",\"Force\":3.1,\"ForceMax\":7.2}},\"MeasurementHistory\":[{\"MeasureTime\":\"2018-05-07T12:10:03\", \"Precipitation\":{\"AmountName\":\"Ingen nederb\u00F6rd\",\"Type\":\"Ingen nederb\u00F6rd\",\"TypeIconId\":\"precipitationNoPrecipitation\"}, \"Road\":{\"Temp\":34.1,\"TempIconId\":\"tempAirRoad\"}, \"Air\":{\"Temp\":22.7,\"TempIconId\":\"tempAirRoad\",\"RelativeHumidity\":35.5}, \"Wind\":{\"Direction\":135,\"DirectionIconId\":\"windSE\",\"DirectionText\":\"Syd\u00F6st\",\"Force\":3.1,\"ForceMax\":7.2}},{\"MeasureTime\":\"2018-05-07T12:00:03\", \"Precipitation\":{\"AmountName\":\"Ingen nederb\u00F6rd\",\"Type\":\"Ingen nederb\u00F6rd\",\"TypeIconId\":\"precipitationNoPrecipitation\"}, \"Road\":{\"Temp\":34,\"TempIconId\":\"tempAirRoad\"}, \"Air\":{\"Temp\":22.8,\"TempIconId\":\"tempAirRoad\",\"RelativeHumidity\":36.8}, \"Wind\":{\"Direction\":135,\"DirectionIconId\":\"windSE\",\"DirectionText\":\"Syd\u00F6st\",\"Force\":2.9,\"ForceMax\":7.2}},{\"MeasureTime\":\"2018-05-07T11:50:04\", \"Precipitation\":{\"AmountName\":\"Ingen nederb\u00F6rd\",\"Type\":\"Ingen nederb\u00F6rd\",\"TypeIconId\":\"precipitationNoPrecipitation\"}, \"Road\":{\"Temp\":34,\"TempIconId\":\"tempAirRoad\"}, \"Air\":{\"Temp\":22.6,\"TempIconId\":\"tempAirRoad\",\"RelativeHumidity\":35.5}, \"Wind\":{\"Direction\":135,\"DirectionIconId\":\"windSE\",\"DirectionText\":\"Syd\u00F6st\",\"Force\":2.8,\"ForceMax\":7.2}},{\"MeasureTime\":\"2018-05-07T11:40:03\", \"Precipitation\":{\"AmountName\":\"Ingen nederb\u00F6rd\",\"Type\":\"Ingen nederb\u00F6rd\",\"TypeIconId\":\"precipitationNoPrecipitation\"}, \"Road\":{\"Temp\":33.5,\"TempIconId\":\"tempAirRoad\"}, \"Air\":{\"Temp\":22.7,\"TempIconId\":\"tempAirRoad\",\"RelativeHumidity\":37.9}, \"Wind\":{\"Direction\":135,\"DirectionIconId\":\"windSE\",\"DirectionText\":\"Syd\u00F6st\",\"Force\":2.6,\"ForceMax\":6.2}},{\"MeasureTime\":\"2018-05-07T11:30:00\", \"Precipitation\":{\"AmountName\":\"Ingen nederb\u00F6rd\",\"Type\":\"Ingen nederb\u00F6rd\",\"TypeIconId\":\"precipitationNoPrecipitation\"}, \"Road\":{\"Temp\":33,\"TempIconId\":\"tempAirRoad\"}, \"Air\":{\"Temp\":22.6,\"TempIconId\":\"tempAirRoad\",\"RelativeHumidity\":35.5}, \"Wind\":{\"Direction\":135,\"DirectionIconId\":\"windSE\",\"DirectionText\":\"Syd\u00F6st\",\"Force\":2.5,\"ForceMax\":6.2}}],\"ModifiedTime\":\"2018-05-07T10:15:16.603Z\",\"Name\":\"Eker\u00F6\",\"RoadNumberNumeric\":816}]}]}}");

            return mock;
        }

        private class MockContainer
        {
            public Mock<ILogger<WeatherService>> Logger { get; set; }
            public MockHttpMessageHandler Http { get; set; }

            public WeatherService Service { get; set; }
        }
    }
}
