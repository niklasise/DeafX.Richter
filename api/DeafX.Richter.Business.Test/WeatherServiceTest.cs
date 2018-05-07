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
            //public string CookieValue { get; set; } = "ZWAYSession=5ba64edf-3cdd-cf70-a860-f0c76e7c1dc4";
            //public string GetAllDevicesJson { get; set; } = "{\"data\":{\"structureChanged\":true,\"updateTime\":1509225432,\"devices\":[{\"creationTime\":1459282629,\"creatorId\":8,\"customIcons\":{},\"deviceType\":\"battery\",\"h\":-592588977,\"hasHistory\":false,\"id\":\"BatteryPolling_8\",\"location\":0,\"metrics\":{\"probeTitle\":\"Battery\",\"scaleTitle\":\"%\",\"title\":\"Battery digest 8\",\"level\":100},\"order\":{\"rooms\":0,\"elements\":0,\"dashboard\":0},\"permanently_hidden\":false,\"probeType\":\"\",\"tags\":[],\"visibility\":true,\"updateTime\":1509225432},{\"creationTime\":1509225429,\"creatorId\":5,\"customIcons\":{},\"deviceType\":\"text\",\"h\":-1261400328,\"hasHistory\":false,\"id\":\"InfoWidget_5_Int\",\"location\":0,\"metrics\":{\"title\":\"Welcome to your Smart Home!\",\"text\":\"This interface allows managing your Smart Home equipped with interconnected Z-Wave devices. It will show every function of the device as one single <strong>Element</strong> (In case a physical device has multiple functions like switching and metering \u2013 it will generate multiple elements). All elements are listed in the <strong>Element View</strong> and can be filtered by function type (switch, dimmer, sensor) or other filtering criteria. <br><br>Each Element has an <strong>Element Configuration Dialog</strong> to rename it or to hide it in case was created but it is not needed. Important elements can be marked to be shown in the <strong>Dashboard</strong>. Additionally the elements can be grouped into rooms. <br><br>Every change of a sensor value or a switching status is called an <strong>Event</strong> and is shown in the <strong>Timeline</strong>. Filtering allows monitoring the changes of one single function or device. <br><br>All other functions such as time triggered actions, the use of information from the Internet, scenes plugin of other technologies and service are realized in <strong>Apps</strong>. These apps can create none, one or multiple new elements and events. The menu <strong>Settings > Apps</strong> allows downloading, activating and configuring your Apps. <br><br>To Add a new device please follow the instruction under <strong>Settings > Devices</strong>.\",\"icon\":\"app/img/logo-z-wave-z-only.png\"},\"order\":{\"rooms\":0,\"elements\":0,\"dashboard\":0},\"permanently_hidden\":false,\"probeType\":\"\",\"tags\":[],\"visibility\":true,\"updateTime\":1509225429},{\"creationTime\":1509225429,\"creatorId\":6,\"customIcons\":{},\"deviceType\":\"text\",\"h\":-1260476807,\"hasHistory\":false,\"id\":\"InfoWidget_6_Int\",\"location\":0,\"metrics\":{\"title\":\"Dear Expert User\",\"text\":\"<center>If you still want to use ExpertUI please go, after you are successfully logged in, to <br><strong> Menu > Open ExpertUI </strong> <br> or call <br><strong> http://MYRASP:8083/expert </strong><br> in your browser. <br> <br>You could hide or remove this widget in menu <br><strong>Apps > Active Tab</strong>. </center>\",\"icon\":\"app/img/logo-z-wave-z-only.png\"},\"order\":{\"rooms\":0,\"elements\":0,\"dashboard\":0},\"permanently_hidden\":false,\"probeType\":\"\",\"tags\":[],\"visibility\":true,\"updateTime\":1509225429},{\"creationTime\":1459356175,\"customIcons\":{},\"deviceType\":\"switchControl\",\"h\":1321774902,\"hasHistory\":false,\"id\":\"ZWayVDev_zway_Remote_3-1-1-B\",\"location\":0,\"metrics\":{\"icon\":\"gesture\",\"title\":\"Fibar Group (3.1.1) Button\",\"level\":\"\",\"change\":\"\",\"isFailed\":true},\"order\":{\"rooms\":0,\"elements\":0,\"dashboard\":0},\"permanently_hidden\":false,\"probeType\":\"\",\"tags\":[],\"visibility\":true,\"updateTime\":1509225429},{\"creationTime\":1459276234,\"creatorId\":1,\"customIcons\":{},\"deviceType\":\"switchBinary\",\"h\":1078449915,\"hasHistory\":false,\"id\":\"ZWayVDev_zway_2-0-37\",\"location\":0,\"metrics\":{\"icon\":\"switch\",\"title\":\"Fibar Group (2.0) Switch\",\"level\":\"on\",\"isFailed\":true},\"order\":{\"rooms\":0,\"elements\":0,\"dashboard\":0},\"permanently_hidden\":false,\"probeType\":\"\",\"tags\":[],\"visibility\":true,\"updateTime\":1509385213},{\"creationTime\":1459276234,\"creatorId\":1,\"customIcons\":{},\"deviceType\":\"sensorMultilevel\",\"h\":1303283139,\"hasHistory\":false,\"id\":\"ZWayVDev_zway_2-0-49-4\",\"location\":0,\"metrics\":{\"probeTitle\":\"Power\",\"scaleTitle\":\"W\",\"level\":0,\"icon\":\"energy\",\"title\":\"Fibar Group Power (2.0.49.4)\",\"isFailed\":true},\"order\":{\"rooms\":0,\"elements\":0,\"dashboard\":0},\"permanently_hidden\":false,\"probeType\":\"energy\",\"tags\":[],\"visibility\":true,\"updateTime\":1509385213},{\"creationTime\":1459276235,\"creatorId\":1,\"customIcons\":{},\"deviceType\":\"sensorMultilevel\",\"h\":1303304277,\"hasHistory\":false,\"id\":\"ZWayVDev_zway_2-0-50-0\",\"location\":0,\"metrics\":{\"probeTitle\":\"Electric\",\"scaleTitle\":\"kWh\",\"level\":0.62,\"icon\":\"meter\",\"title\":\"Fibar Group Electric (2.0.50.0) Meter\",\"isFailed\":true},\"order\":{\"rooms\":0,\"elements\":0,\"dashboard\":0},\"permanently_hidden\":false,\"probeType\":\"meterElectric_kilowatt_hour\",\"tags\":[],\"visibility\":true,\"updateTime\":1509385213},{\"creationTime\":1459276235,\"creatorId\":1,\"customIcons\":{},\"deviceType\":\"sensorMultilevel\",\"h\":1303304279,\"hasHistory\":false,\"id\":\"ZWayVDev_zway_2-0-50-2\",\"location\":0,\"metrics\":{\"probeTitle\":\"Electric\",\"scaleTitle\":\"W\",\"level\":0,\"icon\":\"meter\",\"title\":\"Fibar Group Electric (2.0.50.2) Meter\",\"isFailed\":true},\"order\":{\"rooms\":0,\"elements\":0,\"dashboard\":0},\"permanently_hidden\":false,\"probeType\":\"meterElectric_watt\",\"tags\":[],\"visibility\":true,\"updateTime\":1509385213},{\"creationTime\":1459356136,\"creatorId\":1,\"customIcons\":{},\"deviceType\":\"sensorBinary\",\"h\":-1248874786,\"hasHistory\":false,\"id\":\"ZWayVDev_zway_3-0-48-1\",\"location\":0,\"metrics\":{\"probeTitle\":\"General purpose\",\"scaleTitle\":\"\",\"icon\":\"motion\",\"level\":\"on\",\"title\":\"Fibar Group General purpose (3.0.48.1)\",\"isFailed\":false},\"order\":{\"rooms\":0,\"elements\":0,\"dashboard\":0},\"permanently_hidden\":false,\"probeType\":\"general_purpose\",\"tags\":[],\"visibility\":true,\"updateTime\":1509225431},{\"creationTime\":1459356137,\"creatorId\":1,\"customIcons\":{},\"deviceType\":\"sensorMultilevel\",\"h\":-1248873825,\"hasHistory\":false,\"id\":\"ZWayVDev_zway_3-0-49-1\",\"location\":0,\"metrics\":{\"probeTitle\":\"Temperature\",\"scaleTitle\":\"\u00B0C\",\"level\":22.3,\"icon\":\"temperature\",\"title\":\"Fibar Group Temperature (3.0.49.1)\",\"isFailed\":true},\"order\":{\"rooms\":0,\"elements\":0,\"dashboard\":0},\"permanently_hidden\":false,\"probeType\":\"temperature\",\"tags\":[],\"visibility\":true,\"updateTime\":1509225431},{\"creationTime\":1459356137,\"creatorId\":1,\"customIcons\":{},\"deviceType\":\"sensorMultilevel\",\"h\":-1248873823,\"hasHistory\":false,\"id\":\"ZWayVDev_zway_3-0-49-3\",\"location\":0,\"metrics\":{\"probeTitle\":\"Luminiscence\",\"scaleTitle\":\"Lux\",\"level\":2,\"icon\":\"luminosity\",\"title\":\"Fibar Group Luminiscence (3.0.49.3)\",\"isFailed\":true},\"order\":{\"rooms\":0,\"elements\":0,\"dashboard\":0},\"permanently_hidden\":false,\"probeType\":\"luminosity\",\"tags\":[],\"visibility\":true,\"updateTime\":1509225431},{\"creationTime\":1459356136,\"creatorId\":1,\"customIcons\":{},\"deviceType\":\"battery\",\"h\":-40289343,\"hasHistory\":false,\"id\":\"ZWayVDev_zway_3-0-128\",\"location\":0,\"metrics\":{\"probeTitle\":\"Battery\",\"scaleTitle\":\"%\",\"level\":100,\"icon\":\"battery\",\"title\":\"Fibar Group Battery (3.0)\",\"isFailed\":true},\"order\":{\"rooms\":0,\"elements\":0,\"dashboard\":0},\"permanently_hidden\":false,\"probeType\":\"\",\"tags\":[],\"visibility\":true,\"updateTime\":1509225432}]},\"code\":200,\"message\":\"200 OK\",\"error\":null}";
            //public string BaseAdress { get; set; } = "http://test/api";
            //public string Username { get; set; } = "MockUser";
            //public string Password { get; set; } = "MockPassword";

            public bool HasUpdates { get; set; }

            public WeatherConfiguration Configuration { get; set; } = new WeatherConfiguration()
            {
                ApiKey = "abc123",
                ApiUrl = "http://test.api/json",
                StationId = "def456",
                UpdateInterval = 1000
            };
            //public ZWayDeviceConfiguration[] DeviceConfiguration { get; set; } = new ZWayDeviceConfiguration[] {
            //    new ZWayDeviceConfiguration()
            //    {
            //        Id = "Device1",
            //        Type = "sensor",
            //        ZWayId = "ZWayVDev_zway_3-0-49-1",
            //    },
            //    new ZWayDeviceConfiguration()
            //    {
            //        Id = "Device2",
            //        Type = "powerplug",
            //        ZWayId = "ZWayVDev_zway_2-0-37",
            //        ZWayPowerId = "ZWayVDev_zway_2-0-49-4"
            //    }
            //};
            //public bool PerformDeviceUpdates { get; set; }
        }


        [TestMethod]
        public async Task Init()
        {
            var data = new MockData();
            var container = GetMockContainer(data);

            var result = await container.Service.InitAsync(data.Configuration);

            Assert.IsTrue(result);

            var airDevice = container.Service.GetDevice("WeatherDevice_Air") as WeatherAirDevice;
            var roadDevice = container.Service.GetDevice("WeatherDevice_Road") as WeatherRoadDevice;
            var precipitationDevice = container.Service.GetDevice("WeatherDevice_Precipitation") as WeatherPercipitationDevice;
            var windDevice = container.Service.GetDevice("WeatherDevice_Wind") as WeatherWindDevice;

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

            var airDevice = result.First(d => d.Id == "WeatherDevice_Air") as WeatherAirDevice;
            var roadDevice = result.First(d => d.Id == "WeatherDevice_Road") as WeatherRoadDevice;
            var precipitationDevice = result.First(d => d.Id == "WeatherDevice_Precipitation") as WeatherPercipitationDevice;
            var windDevice = result.First(d => d.Id == "WeatherDevice_Wind") as WeatherWindDevice;

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

            var updatedDevicesQueue = new Queue<string>();

            updatedDevicesQueue.Enqueue("");
            updatedDevicesQueue.Enqueue("");

            mock.Expect(HttpMethod.Post, data.Configuration.ApiUrl).Respond("application/json", "{\"RESPONSE\":{\"RESULT\":[{\"WeatherStation\":[{\"Active\":true,\"CountyNo\":[2,1], \"Geometry\":{\"SWEREF99TM\":\"POINT (657232.31 6574253.87)\",\"WGS84\":\"POINT (17.759519577026367 59.277908325195313)\"},\"IconId\":\"weatherStation\",\"Id\":\"SE_STA_VVIS241\", \"Measurement\":{\"MeasureTime\":\"2018-05-07T12:10:03\", \"Precipitation\":{\"AmountName\":\"Ingen nederb\u00F6rd\",\"Type\":\"Ingen nederb\u00F6rd\",\"TypeIconId\":\"precipitationNoPrecipitation\"}, \"Road\":{\"Temp\":34.1,\"TempIconId\":\"tempAirRoad\"}, \"Air\":{\"Temp\":22.7,\"TempIconId\":\"tempAirRoad\",\"RelativeHumidity\":35.5}, \"Wind\":{\"Direction\":135,\"DirectionIconId\":\"windSE\",\"DirectionText\":\"Syd\u00F6st\",\"Force\":3.1,\"ForceMax\":7.2}},\"MeasurementHistory\":[{\"MeasureTime\":\"2018-05-07T12:10:03\", \"Precipitation\":{\"AmountName\":\"Ingen nederb\u00F6rd\",\"Type\":\"Ingen nederb\u00F6rd\",\"TypeIconId\":\"precipitationNoPrecipitation\"}, \"Road\":{\"Temp\":34.1,\"TempIconId\":\"tempAirRoad\"}, \"Air\":{\"Temp\":22.7,\"TempIconId\":\"tempAirRoad\",\"RelativeHumidity\":35.5}, \"Wind\":{\"Direction\":135,\"DirectionIconId\":\"windSE\",\"DirectionText\":\"Syd\u00F6st\",\"Force\":3.1,\"ForceMax\":7.2}},{\"MeasureTime\":\"2018-05-07T12:00:03\", \"Precipitation\":{\"AmountName\":\"Ingen nederb\u00F6rd\",\"Type\":\"Ingen nederb\u00F6rd\",\"TypeIconId\":\"precipitationNoPrecipitation\"}, \"Road\":{\"Temp\":34,\"TempIconId\":\"tempAirRoad\"}, \"Air\":{\"Temp\":22.8,\"TempIconId\":\"tempAirRoad\",\"RelativeHumidity\":36.8}, \"Wind\":{\"Direction\":135,\"DirectionIconId\":\"windSE\",\"DirectionText\":\"Syd\u00F6st\",\"Force\":2.9,\"ForceMax\":7.2}},{\"MeasureTime\":\"2018-05-07T11:50:04\", \"Precipitation\":{\"AmountName\":\"Ingen nederb\u00F6rd\",\"Type\":\"Ingen nederb\u00F6rd\",\"TypeIconId\":\"precipitationNoPrecipitation\"}, \"Road\":{\"Temp\":34,\"TempIconId\":\"tempAirRoad\"}, \"Air\":{\"Temp\":22.6,\"TempIconId\":\"tempAirRoad\",\"RelativeHumidity\":35.5}, \"Wind\":{\"Direction\":135,\"DirectionIconId\":\"windSE\",\"DirectionText\":\"Syd\u00F6st\",\"Force\":2.8,\"ForceMax\":7.2}},{\"MeasureTime\":\"2018-05-07T11:40:03\", \"Precipitation\":{\"AmountName\":\"Ingen nederb\u00F6rd\",\"Type\":\"Ingen nederb\u00F6rd\",\"TypeIconId\":\"precipitationNoPrecipitation\"}, \"Road\":{\"Temp\":33.5,\"TempIconId\":\"tempAirRoad\"}, \"Air\":{\"Temp\":22.7,\"TempIconId\":\"tempAirRoad\",\"RelativeHumidity\":37.9}, \"Wind\":{\"Direction\":135,\"DirectionIconId\":\"windSE\",\"DirectionText\":\"Syd\u00F6st\",\"Force\":2.6,\"ForceMax\":6.2}},{\"MeasureTime\":\"2018-05-07T11:30:00\", \"Precipitation\":{\"AmountName\":\"Ingen nederb\u00F6rd\",\"Type\":\"Ingen nederb\u00F6rd\",\"TypeIconId\":\"precipitationNoPrecipitation\"}, \"Road\":{\"Temp\":33,\"TempIconId\":\"tempAirRoad\"}, \"Air\":{\"Temp\":22.6,\"TempIconId\":\"tempAirRoad\",\"RelativeHumidity\":35.5}, \"Wind\":{\"Direction\":135,\"DirectionIconId\":\"windSE\",\"DirectionText\":\"Syd\u00F6st\",\"Force\":2.5,\"ForceMax\":6.2}}],\"ModifiedTime\":\"2018-05-07T10:15:16.603Z\",\"Name\":\"Eker\u00F6\",\"RoadNumberNumeric\":816}]}]}}");

            if (data.HasUpdates)
            {
                mock.Expect(HttpMethod.Post, data.Configuration.ApiUrl).Respond("application/json", "{\"RESPONSE\":{\"RESULT\":[{\"WeatherStation\":[{\"Active\":true,\"CountyNo\":[2,1],\"Geometry\":{\"SWEREF99TM\":\"POINT (657232.31 6574253.87)\",\"WGS84\":\"POINT (17.759519577026367 59.277908325195313)\"},\"IconId\":\"weatherStation\",\"Id\":\"SE_STA_VVIS241\",\"Measurement\":{\"MeasureTime\":\"2018-05-07T12:10:03\",\"Precipitation\":{\"AmountName\":\"Mkt Regn\",\"Amount\":2.2,\"Type\":\"Regn\",\"TypeIconId\":\"precipitationNoPrecipitation\"},\"Road\":{\"Temp\":33.1,\"TempIconId\":\"tempAirRoad\"},\"Air\":{\"Temp\":21.7,\"TempIconId\":\"tempAirRoad\",\"RelativeHumidity\":40.5},\"Wind\":{\"Direction\":180,\"DirectionIconId\":\"windS\",\"DirectionText\":\"Syd\",\"Force\":1.1,\"ForceMax\":3.4}},\"MeasurementHistory\":[{\"MeasureTime\":\"2018-05-07T12:10:03\",\"Precipitation\":{\"AmountName\":\"Ingen nederb\u00F6rd\",\"Type\":\"Ingen nederb\u00F6rd\",\"TypeIconId\":\"precipitationNoPrecipitation\"},\"Road\":{\"Temp\":34.1,\"TempIconId\":\"tempAirRoad\"},\"Air\":{\"Temp\":22.7,\"TempIconId\":\"tempAirRoad\",\"RelativeHumidity\":35.5},\"Wind\":{\"Direction\":135,\"DirectionIconId\":\"windSE\",\"DirectionText\":\"Syd\u00F6st\",\"Force\":3.1,\"ForceMax\":7.2}},{\"MeasureTime\":\"2018-05-07T12:00:03\",\"Precipitation\":{\"AmountName\":\"Ingen nederb\u00F6rd\",\"Type\":\"Ingen nederb\u00F6rd\",\"TypeIconId\":\"precipitationNoPrecipitation\"},\"Road\":{\"Temp\":34,\"TempIconId\":\"tempAirRoad\"},\"Air\":{\"Temp\":22.8,\"TempIconId\":\"tempAirRoad\",\"RelativeHumidity\":36.8},\"Wind\":{\"Direction\":135,\"DirectionIconId\":\"windSE\",\"DirectionText\":\"Syd\u00F6st\",\"Force\":2.9,\"ForceMax\":7.2}},{\"MeasureTime\":\"2018-05-07T11:50:04\",\"Precipitation\":{\"AmountName\":\"Ingen nederb\u00F6rd\",\"Type\":\"Ingen nederb\u00F6rd\",\"TypeIconId\":\"precipitationNoPrecipitation\"},\"Road\":{\"Temp\":34,\"TempIconId\":\"tempAirRoad\"},\"Air\":{\"Temp\":22.6,\"TempIconId\":\"tempAirRoad\",\"RelativeHumidity\":35.5},\"Wind\":{\"Direction\":135,\"DirectionIconId\":\"windSE\",\"DirectionText\":\"Syd\u00F6st\",\"Force\":2.8,\"ForceMax\":7.2}},{\"MeasureTime\":\"2018-05-07T11:40:03\",\"Precipitation\":{\"AmountName\":\"Ingen nederb\u00F6rd\",\"Type\":\"Ingen nederb\u00F6rd\",\"TypeIconId\":\"precipitationNoPrecipitation\"},\"Road\":{\"Temp\":33.5,\"TempIconId\":\"tempAirRoad\"},\"Air\":{\"Temp\":22.7,\"TempIconId\":\"tempAirRoad\",\"RelativeHumidity\":37.9},\"Wind\":{\"Direction\":135,\"DirectionIconId\":\"windSE\",\"DirectionText\":\"Syd\u00F6st\",\"Force\":2.6,\"ForceMax\":6.2}},{\"MeasureTime\":\"2018-05-07T11:30:00\",\"Precipitation\":{\"AmountName\":\"Ingen nederb\u00F6rd\",\"Type\":\"Ingen nederb\u00F6rd\",\"TypeIconId\":\"precipitationNoPrecipitation\"},\"Road\":{\"Temp\":33,\"TempIconId\":\"tempAirRoad\"},\"Air\":{\"Temp\":22.6,\"TempIconId\":\"tempAirRoad\",\"RelativeHumidity\":35.5},\"Wind\":{\"Direction\":135,\"DirectionIconId\":\"windSE\",\"DirectionText\":\"Syd\u00F6st\",\"Force\":2.5,\"ForceMax\":6.2}}],\"ModifiedTime\":\"2018-05-07T10:15:16.603Z\",\"Name\":\"Eker\u00F6\",\"RoadNumberNumeric\":816}]}]}}");
            }

            mock.When(HttpMethod.Post, data.Configuration.ApiUrl).Respond("application/json", "{\"RESPONSE\":{\"RESULT\":[{\"WeatherStation\":[{\"Active\":true,\"CountyNo\":[2,1], \"Geometry\":{\"SWEREF99TM\":\"POINT (657232.31 6574253.87)\",\"WGS84\":\"POINT (17.759519577026367 59.277908325195313)\"},\"IconId\":\"weatherStation\",\"Id\":\"SE_STA_VVIS241\", \"Measurement\":{\"MeasureTime\":\"2018-05-07T12:10:03\", \"Precipitation\":{\"AmountName\":\"Ingen nederb\u00F6rd\",\"Type\":\"Ingen nederb\u00F6rd\",\"TypeIconId\":\"precipitationNoPrecipitation\"}, \"Road\":{\"Temp\":34.1,\"TempIconId\":\"tempAirRoad\"}, \"Air\":{\"Temp\":22.7,\"TempIconId\":\"tempAirRoad\",\"RelativeHumidity\":35.5}, \"Wind\":{\"Direction\":135,\"DirectionIconId\":\"windSE\",\"DirectionText\":\"Syd\u00F6st\",\"Force\":3.1,\"ForceMax\":7.2}},\"MeasurementHistory\":[{\"MeasureTime\":\"2018-05-07T12:10:03\", \"Precipitation\":{\"AmountName\":\"Ingen nederb\u00F6rd\",\"Type\":\"Ingen nederb\u00F6rd\",\"TypeIconId\":\"precipitationNoPrecipitation\"}, \"Road\":{\"Temp\":34.1,\"TempIconId\":\"tempAirRoad\"}, \"Air\":{\"Temp\":22.7,\"TempIconId\":\"tempAirRoad\",\"RelativeHumidity\":35.5}, \"Wind\":{\"Direction\":135,\"DirectionIconId\":\"windSE\",\"DirectionText\":\"Syd\u00F6st\",\"Force\":3.1,\"ForceMax\":7.2}},{\"MeasureTime\":\"2018-05-07T12:00:03\", \"Precipitation\":{\"AmountName\":\"Ingen nederb\u00F6rd\",\"Type\":\"Ingen nederb\u00F6rd\",\"TypeIconId\":\"precipitationNoPrecipitation\"}, \"Road\":{\"Temp\":34,\"TempIconId\":\"tempAirRoad\"}, \"Air\":{\"Temp\":22.8,\"TempIconId\":\"tempAirRoad\",\"RelativeHumidity\":36.8}, \"Wind\":{\"Direction\":135,\"DirectionIconId\":\"windSE\",\"DirectionText\":\"Syd\u00F6st\",\"Force\":2.9,\"ForceMax\":7.2}},{\"MeasureTime\":\"2018-05-07T11:50:04\", \"Precipitation\":{\"AmountName\":\"Ingen nederb\u00F6rd\",\"Type\":\"Ingen nederb\u00F6rd\",\"TypeIconId\":\"precipitationNoPrecipitation\"}, \"Road\":{\"Temp\":34,\"TempIconId\":\"tempAirRoad\"}, \"Air\":{\"Temp\":22.6,\"TempIconId\":\"tempAirRoad\",\"RelativeHumidity\":35.5}, \"Wind\":{\"Direction\":135,\"DirectionIconId\":\"windSE\",\"DirectionText\":\"Syd\u00F6st\",\"Force\":2.8,\"ForceMax\":7.2}},{\"MeasureTime\":\"2018-05-07T11:40:03\", \"Precipitation\":{\"AmountName\":\"Ingen nederb\u00F6rd\",\"Type\":\"Ingen nederb\u00F6rd\",\"TypeIconId\":\"precipitationNoPrecipitation\"}, \"Road\":{\"Temp\":33.5,\"TempIconId\":\"tempAirRoad\"}, \"Air\":{\"Temp\":22.7,\"TempIconId\":\"tempAirRoad\",\"RelativeHumidity\":37.9}, \"Wind\":{\"Direction\":135,\"DirectionIconId\":\"windSE\",\"DirectionText\":\"Syd\u00F6st\",\"Force\":2.6,\"ForceMax\":6.2}},{\"MeasureTime\":\"2018-05-07T11:30:00\", \"Precipitation\":{\"AmountName\":\"Ingen nederb\u00F6rd\",\"Type\":\"Ingen nederb\u00F6rd\",\"TypeIconId\":\"precipitationNoPrecipitation\"}, \"Road\":{\"Temp\":33,\"TempIconId\":\"tempAirRoad\"}, \"Air\":{\"Temp\":22.6,\"TempIconId\":\"tempAirRoad\",\"RelativeHumidity\":35.5}, \"Wind\":{\"Direction\":135,\"DirectionIconId\":\"windSE\",\"DirectionText\":\"Syd\u00F6st\",\"Force\":2.5,\"ForceMax\":6.2}}],\"ModifiedTime\":\"2018-05-07T10:15:16.603Z\",\"Name\":\"Eker\u00F6\",\"RoadNumberNumeric\":816}]}]}}");


            //mock.Expect(HttpMethod.Post, "http://test/api/login").Respond(req =>
            //{
            //    var httpResponse = new HttpResponseMessage();

            //    httpResponse.Headers.Add("Set-Cookie", data.CookieValue + "; Path=/; HttpOnly");
            //    httpResponse.StatusCode = HttpStatusCode.OK;

            //    return httpResponse;
            //});

            //mock.Expect(HttpMethod.Get, "http://test/api/devices").WithHeaders("Cookie", data.CookieValue).Respond("application/json", data.GetAllDevicesJson);

            //if (data.PerformDeviceUpdates)
            //{
            //    mock.Expect(HttpMethod.Get, "http://test/api/devices").WithHeaders("Cookie", data.CookieValue).WithQueryString("since=1509225432").Respond("application/json", "{\"data\":{\"structureChanged\":false,\"updateTime\":1509225433,\"devices\":[{\"creationTime\":1459356137,\"creatorId\":1,\"customIcons\":{},\"deviceType\":\"sensorMultilevel\",\"h\":-1248873825,\"hasHistory\":false,\"id\":\"ZWayVDev_zway_3-0-49-1\",\"location\":0,\"metrics\":{\"probeTitle\":\"Temperature\",\"scaleTitle\":\"\u00B0C\",\"level\":23.5,\"icon\":\"temperature\",\"title\":\"Fibar Group Temperature (3.0.49.1)\",\"isFailed\":true},\"order\":{\"rooms\":0,\"elements\":0,\"dashboard\":0},\"permanently_hidden\":false,\"probeType\":\"temperature\",\"tags\":[],\"visibility\":true,\"updateTime\":1509225431}]},\"code\":200,\"message\":\"200 OK\", \"error\":null}");
            //    mock.Expect(HttpMethod.Get, "http://test/api/devices").WithHeaders("Cookie", data.CookieValue).WithExactQueryString("since=1509225433").Respond("application/json", "{\"data\":{\"structureChanged\":false,\"updateTime\":1509225434,\"devices\":[{\"creationTime\":1459276234,\"creatorId\":1,\"customIcons\":{},\"deviceType\":\"sensorMultilevel\",\"h\":1303283139,\"hasHistory\":false,\"id\":\"ZWayVDev_zway_2-0-49-4\",\"location\":0,\"metrics\":{\"probeTitle\":\"Power\",\"scaleTitle\":\"W\",\"level\":20,\"icon\":\"energy\",\"title\":\"Fibar Group Power (2.0.49.4)\",\"isFailed\":true},\"order\":{\"rooms\":0,\"elements\":0,\"dashboard\":0},\"permanently_hidden\":false,\"probeType\":\"energy\",\"tags\":[],\"visibility\":true,\"updateTime\":1509385213}]},\"code\":200,\"message\":\"200 OK\", \"error\":null}");
            //    mock.Expect(HttpMethod.Get, "http://test/api/devices").WithHeaders("Cookie", data.CookieValue).WithExactQueryString("since=1509225434").Respond("application/json", "{\"data\":{\"structureChanged\":false,\"updateTime\":1509225435,\"devices\":[{\"creationTime\":1459276234,\"creatorId\":1,\"customIcons\":{},\"deviceType\":\"switchBinary\",\"h\":1078449915,\"hasHistory\":false,\"id\":\"ZWayVDev_zway_2-0-37\",\"location\":0,\"metrics\":{\"icon\":\"switch\",\"title\":\"Fibar Group (2.0) Switch\",\"level\":\"off\",\"isFailed\":true},\"order\":{\"rooms\":0,\"elements\":0,\"dashboard\":0},\"permanently_hidden\":false,\"probeType\":\"\",\"tags\":[],\"visibility\":true,\"updateTime\":1509385213}]},\"code\":200,\"message\":\"200 OK\", \"error\":null}");
            //    mock.When(HttpMethod.Get, "http://test/api/devices").WithHeaders("Cookie", data.CookieValue).WithExactQueryString("since=1509225435").Respond("application/json", "{\"data\":{\"structureChanged\":false,\"updateTime\":1509225435,\"devices\":[]},\"code\":200,\"message\":\"200 OK\", \"error\":null}");
            //}

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
