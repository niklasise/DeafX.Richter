using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DeafX.Richter.Business.Services;
using Moq;
using Microsoft.Extensions.Logging;
using RichardSzalay.MockHttp;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using DeafX.Richter.Business.Models;
using System.Threading.Tasks;
using System.Linq;
using DeafX.Richter.Business.Interfaces;
using System.Collections.Generic;

namespace DeafX.Richter.Business.Test
{
    [TestClass]
    public class ZWayServiceTest
    {
        private class MockData
        {
            public string CookieValue { get; set; } = "ZWAYSession=5ba64edf-3cdd-cf70-a860-f0c76e7c1dc4";
            public string GetAllDevicesJson { get; set; } = "{\"data\":{\"structureChanged\":true,\"updateTime\":1509225432,\"devices\":[{\"creationTime\":1459282629,\"creatorId\":8,\"customIcons\":{},\"deviceType\":\"battery\",\"h\":-592588977,\"hasHistory\":false,\"id\":\"BatteryPolling_8\",\"location\":0,\"metrics\":{\"probeTitle\":\"Battery\",\"scaleTitle\":\"%\",\"title\":\"Battery digest 8\",\"level\":100},\"order\":{\"rooms\":0,\"elements\":0,\"dashboard\":0},\"permanently_hidden\":false,\"probeType\":\"\",\"tags\":[],\"visibility\":true,\"updateTime\":1509225432},{\"creationTime\":1509225429,\"creatorId\":5,\"customIcons\":{},\"deviceType\":\"text\",\"h\":-1261400328,\"hasHistory\":false,\"id\":\"InfoWidget_5_Int\",\"location\":0,\"metrics\":{\"title\":\"Welcome to your Smart Home!\",\"text\":\"This interface allows managing your Smart Home equipped with interconnected Z-Wave devices. It will show every function of the device as one single <strong>Element</strong> (In case a physical device has multiple functions like switching and metering \u2013 it will generate multiple elements). All elements are listed in the <strong>Element View</strong> and can be filtered by function type (switch, dimmer, sensor) or other filtering criteria. <br><br>Each Element has an <strong>Element Configuration Dialog</strong> to rename it or to hide it in case was created but it is not needed. Important elements can be marked to be shown in the <strong>Dashboard</strong>. Additionally the elements can be grouped into rooms. <br><br>Every change of a sensor value or a switching status is called an <strong>Event</strong> and is shown in the <strong>Timeline</strong>. Filtering allows monitoring the changes of one single function or device. <br><br>All other functions such as time triggered actions, the use of information from the Internet, scenes plugin of other technologies and service are realized in <strong>Apps</strong>. These apps can create none, one or multiple new elements and events. The menu <strong>Settings > Apps</strong> allows downloading, activating and configuring your Apps. <br><br>To Add a new device please follow the instruction under <strong>Settings > Devices</strong>.\",\"icon\":\"app/img/logo-z-wave-z-only.png\"},\"order\":{\"rooms\":0,\"elements\":0,\"dashboard\":0},\"permanently_hidden\":false,\"probeType\":\"\",\"tags\":[],\"visibility\":true,\"updateTime\":1509225429},{\"creationTime\":1509225429,\"creatorId\":6,\"customIcons\":{},\"deviceType\":\"text\",\"h\":-1260476807,\"hasHistory\":false,\"id\":\"InfoWidget_6_Int\",\"location\":0,\"metrics\":{\"title\":\"Dear Expert User\",\"text\":\"<center>If you still want to use ExpertUI please go, after you are successfully logged in, to <br><strong> Menu > Open ExpertUI </strong> <br> or call <br><strong> http://MYRASP:8083/expert </strong><br> in your browser. <br> <br>You could hide or remove this widget in menu <br><strong>Apps > Active Tab</strong>. </center>\",\"icon\":\"app/img/logo-z-wave-z-only.png\"},\"order\":{\"rooms\":0,\"elements\":0,\"dashboard\":0},\"permanently_hidden\":false,\"probeType\":\"\",\"tags\":[],\"visibility\":true,\"updateTime\":1509225429},{\"creationTime\":1459356175,\"customIcons\":{},\"deviceType\":\"switchControl\",\"h\":1321774902,\"hasHistory\":false,\"id\":\"ZWayVDev_zway_Remote_3-1-1-B\",\"location\":0,\"metrics\":{\"icon\":\"gesture\",\"title\":\"Fibar Group (3.1.1) Button\",\"level\":\"\",\"change\":\"\",\"isFailed\":true},\"order\":{\"rooms\":0,\"elements\":0,\"dashboard\":0},\"permanently_hidden\":false,\"probeType\":\"\",\"tags\":[],\"visibility\":true,\"updateTime\":1509225429},{\"creationTime\":1459276234,\"creatorId\":1,\"customIcons\":{},\"deviceType\":\"switchBinary\",\"h\":1078449915,\"hasHistory\":false,\"id\":\"ZWayVDev_zway_2-0-37\",\"location\":0,\"metrics\":{\"icon\":\"switch\",\"title\":\"Fibar Group (2.0) Switch\",\"level\":\"on\",\"isFailed\":true},\"order\":{\"rooms\":0,\"elements\":0,\"dashboard\":0},\"permanently_hidden\":false,\"probeType\":\"\",\"tags\":[],\"visibility\":true,\"updateTime\":1509385213},{\"creationTime\":1459276234,\"creatorId\":1,\"customIcons\":{},\"deviceType\":\"sensorMultilevel\",\"h\":1303283139,\"hasHistory\":false,\"id\":\"ZWayVDev_zway_2-0-49-4\",\"location\":0,\"metrics\":{\"probeTitle\":\"Power\",\"scaleTitle\":\"W\",\"level\":0,\"icon\":\"energy\",\"title\":\"Fibar Group Power (2.0.49.4)\",\"isFailed\":true},\"order\":{\"rooms\":0,\"elements\":0,\"dashboard\":0},\"permanently_hidden\":false,\"probeType\":\"energy\",\"tags\":[],\"visibility\":true,\"updateTime\":1509385213},{\"creationTime\":1459276235,\"creatorId\":1,\"customIcons\":{},\"deviceType\":\"sensorMultilevel\",\"h\":1303304277,\"hasHistory\":false,\"id\":\"ZWayVDev_zway_2-0-50-0\",\"location\":0,\"metrics\":{\"probeTitle\":\"Electric\",\"scaleTitle\":\"kWh\",\"level\":0.62,\"icon\":\"meter\",\"title\":\"Fibar Group Electric (2.0.50.0) Meter\",\"isFailed\":true},\"order\":{\"rooms\":0,\"elements\":0,\"dashboard\":0},\"permanently_hidden\":false,\"probeType\":\"meterElectric_kilowatt_hour\",\"tags\":[],\"visibility\":true,\"updateTime\":1509385213},{\"creationTime\":1459276235,\"creatorId\":1,\"customIcons\":{},\"deviceType\":\"sensorMultilevel\",\"h\":1303304279,\"hasHistory\":false,\"id\":\"ZWayVDev_zway_2-0-50-2\",\"location\":0,\"metrics\":{\"probeTitle\":\"Electric\",\"scaleTitle\":\"W\",\"level\":0,\"icon\":\"meter\",\"title\":\"Fibar Group Electric (2.0.50.2) Meter\",\"isFailed\":true},\"order\":{\"rooms\":0,\"elements\":0,\"dashboard\":0},\"permanently_hidden\":false,\"probeType\":\"meterElectric_watt\",\"tags\":[],\"visibility\":true,\"updateTime\":1509385213},{\"creationTime\":1459356136,\"creatorId\":1,\"customIcons\":{},\"deviceType\":\"sensorBinary\",\"h\":-1248874786,\"hasHistory\":false,\"id\":\"ZWayVDev_zway_3-0-48-1\",\"location\":0,\"metrics\":{\"probeTitle\":\"General purpose\",\"scaleTitle\":\"\",\"icon\":\"motion\",\"level\":\"on\",\"title\":\"Fibar Group General purpose (3.0.48.1)\",\"isFailed\":false},\"order\":{\"rooms\":0,\"elements\":0,\"dashboard\":0},\"permanently_hidden\":false,\"probeType\":\"general_purpose\",\"tags\":[],\"visibility\":true,\"updateTime\":1509225431},{\"creationTime\":1459356137,\"creatorId\":1,\"customIcons\":{},\"deviceType\":\"sensorMultilevel\",\"h\":-1248873825,\"hasHistory\":false,\"id\":\"ZWayVDev_zway_3-0-49-1\",\"location\":0,\"metrics\":{\"probeTitle\":\"Temperature\",\"scaleTitle\":\"\u00B0C\",\"level\":22.3,\"icon\":\"temperature\",\"title\":\"Fibar Group Temperature (3.0.49.1)\",\"isFailed\":true},\"order\":{\"rooms\":0,\"elements\":0,\"dashboard\":0},\"permanently_hidden\":false,\"probeType\":\"temperature\",\"tags\":[],\"visibility\":true,\"updateTime\":1509225431},{\"creationTime\":1459356137,\"creatorId\":1,\"customIcons\":{},\"deviceType\":\"sensorMultilevel\",\"h\":-1248873823,\"hasHistory\":false,\"id\":\"ZWayVDev_zway_3-0-49-3\",\"location\":0,\"metrics\":{\"probeTitle\":\"Luminiscence\",\"scaleTitle\":\"Lux\",\"level\":2,\"icon\":\"luminosity\",\"title\":\"Fibar Group Luminiscence (3.0.49.3)\",\"isFailed\":true},\"order\":{\"rooms\":0,\"elements\":0,\"dashboard\":0},\"permanently_hidden\":false,\"probeType\":\"luminosity\",\"tags\":[],\"visibility\":true,\"updateTime\":1509225431},{\"creationTime\":1459356136,\"creatorId\":1,\"customIcons\":{},\"deviceType\":\"battery\",\"h\":-40289343,\"hasHistory\":false,\"id\":\"ZWayVDev_zway_3-0-128\",\"location\":0,\"metrics\":{\"probeTitle\":\"Battery\",\"scaleTitle\":\"%\",\"level\":100,\"icon\":\"battery\",\"title\":\"Fibar Group Battery (3.0)\",\"isFailed\":true},\"order\":{\"rooms\":0,\"elements\":0,\"dashboard\":0},\"permanently_hidden\":false,\"probeType\":\"\",\"tags\":[],\"visibility\":true,\"updateTime\":1509225432}]},\"code\":200,\"message\":\"200 OK\",\"error\":null}";
            public string BaseAdress { get; set; } = "http://test/api";
            public string Username { get; set; } = "MockUser";
            public string Password { get; set; } = "MockPassword";
            public ZWayDeviceConfiguration[] DeviceConfiguration { get; set; } = new ZWayDeviceConfiguration[] {
                new ZWayDeviceConfiguration()
                {
                    Id = "Device1",
                    Type = "sensor",
                    ZWayId = "ZWayVDev_zway_3-0-49-1",
                },
                new ZWayDeviceConfiguration()
                {
                    Id = "Device2",
                    Type = "powerplug",
                    ZWayId = "ZWayVDev_zway_2-0-37",
                    ZWayPowerId = "ZWayVDev_zway_2-0-49-4"
                }
            };
            public bool PerformDeviceUpdates { get; set; }
        }


        [TestMethod]
        public async Task Init()
        {
            var data = new MockData();
            var container = GetMockContainer(data);

            var result = await container.Service.InitAsync(new Models.ZWayConfiguration()
            {
                Adress = data.BaseAdress,
                Password = data.Password,
                Username = data.Username,
                Devices = data.DeviceConfiguration
            });

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task GetAllDevices()
        {
            var data = new MockData();
            var container = GetMockContainer(data);

            await container.Service.InitAsync(new Models.ZWayConfiguration()
            {
                Adress = data.BaseAdress,
                Password = data.Password,
                Username = data.Username,
                Devices = data.DeviceConfiguration
            });

            var devices = container.Service.GetAllDevices();

            Assert.AreEqual(2, devices.Length);

            Assert.AreEqual(DeviceValueType.Temperature, devices[0].ValueType);
            Assert.AreEqual(22.3, devices[0].Value);

            Assert.AreEqual(DeviceValueType.Toggle, devices[1].ValueType);
            Assert.AreEqual(true, devices[1].Value);
            Assert.AreEqual("0", (devices[1] as ZWavePowerPlugDevice).Power);
        }

        [TestMethod]
        public async Task OnUpdateDevices()
        {
            var data = new MockData() { PerformDeviceUpdates = true };
            var container = GetMockContainer(data);

            var updateDevices = new List<IDevice[]>();

            container.Service.OnDevicesUpdated += (o, e) =>
            {
                updateDevices.Add(e.UpdatedDevices);
            };

            await container.Service.InitAsync(new Models.ZWayConfiguration()
            {
                Adress = data.BaseAdress,
                Password = data.Password,
                Username = data.Username,
                Devices = data.DeviceConfiguration
            });

            await Task.Delay(4000);

            Assert.AreEqual(1, updateDevices[0].Length);
            Assert.AreEqual("Device1", updateDevices[0][0].Id);
            Assert.AreEqual(23.5, updateDevices[0][0].Value);

            Assert.AreEqual(1, updateDevices[1].Length);
            Assert.AreEqual("Device2", updateDevices[1][0].Id);
            Assert.AreEqual("20", (updateDevices[1][0] as ZWavePowerPlugDevice).Power);

            Assert.AreEqual(1, updateDevices[2].Length);
            Assert.AreEqual("Device2", updateDevices[2][0].Id);
            Assert.AreEqual(false, updateDevices[2][0].Value);
        }

        [TestMethod]
        public async Task GetUpdatedDevices()
        {
            var data = new MockData() { PerformDeviceUpdates = true };
            var container = GetMockContainer(data);

            await container.Service.InitAsync(new Models.ZWayConfiguration()
            {
                Adress = data.BaseAdress,
                Password = data.Password,
                Username = data.Username,
                Devices = data.DeviceConfiguration
            });

            var since = DateTime.Now;

            await Task.Delay(4000);

            var updatedDevices = container.Service.GetUpdatedDevices(since);

            Assert.AreEqual(2, updatedDevices.Length);

            Assert.AreEqual("Device1", updatedDevices[0].Id);
            Assert.AreEqual(23.5, updatedDevices[0].Value);

            Assert.AreEqual("Device2", updatedDevices[1].Id);
            Assert.AreEqual("20", (updatedDevices[1] as ZWavePowerPlugDevice).Power);

            Assert.AreEqual("Device2", updatedDevices[1].Id);
            Assert.AreEqual(false, updatedDevices[1].Value);
        }

        private MockContainer GetMockContainer(MockData data)
        {
            var mockContainer = new MockContainer()
            {
                Logger = new Mock<ILogger<ZWayService>>(),
                Http = GetMockHttpMessageHandler(data)
            };

            mockContainer.Service = new ZWayService(new HttpClient(mockContainer.Http), mockContainer.Logger.Object);

            return mockContainer;
        }

        private MockHttpMessageHandler GetMockHttpMessageHandler(MockData data)
        {
            var mock = new MockHttpMessageHandler();

            var updatedDevicesQueue = new Queue<string>();

            updatedDevicesQueue.Enqueue("");
            updatedDevicesQueue.Enqueue("");

            mock.Expect(HttpMethod.Post, "http://test/api/login").Respond(req =>
            {
                var httpResponse = new HttpResponseMessage();

                httpResponse.Headers.Add("Set-Cookie", data.CookieValue + "; Path=/; HttpOnly");
                httpResponse.StatusCode = HttpStatusCode.OK;

                return httpResponse;
            });

            mock.Expect(HttpMethod.Get, "http://test/api/devices").WithHeaders("Cookie", data.CookieValue).Respond("application/json", data.GetAllDevicesJson);

            if (data.PerformDeviceUpdates)
            {
                mock.Expect(HttpMethod.Get, "http://test/api/devices").WithHeaders("Cookie", data.CookieValue).WithQueryString("since=1509225432").Respond("application/json", "{\"data\":{\"structureChanged\":false,\"updateTime\":1509225433,\"devices\":[{\"creationTime\":1459356137,\"creatorId\":1,\"customIcons\":{},\"deviceType\":\"sensorMultilevel\",\"h\":-1248873825,\"hasHistory\":false,\"id\":\"ZWayVDev_zway_3-0-49-1\",\"location\":0,\"metrics\":{\"probeTitle\":\"Temperature\",\"scaleTitle\":\"\u00B0C\",\"level\":23.5,\"icon\":\"temperature\",\"title\":\"Fibar Group Temperature (3.0.49.1)\",\"isFailed\":true},\"order\":{\"rooms\":0,\"elements\":0,\"dashboard\":0},\"permanently_hidden\":false,\"probeType\":\"temperature\",\"tags\":[],\"visibility\":true,\"updateTime\":1509225431}]},\"code\":200,\"message\":\"200 OK\", \"error\":null}");
                mock.Expect(HttpMethod.Get, "http://test/api/devices").WithHeaders("Cookie", data.CookieValue).WithExactQueryString("since=1509225433").Respond("application/json", "{\"data\":{\"structureChanged\":false,\"updateTime\":1509225434,\"devices\":[{\"creationTime\":1459276234,\"creatorId\":1,\"customIcons\":{},\"deviceType\":\"sensorMultilevel\",\"h\":1303283139,\"hasHistory\":false,\"id\":\"ZWayVDev_zway_2-0-49-4\",\"location\":0,\"metrics\":{\"probeTitle\":\"Power\",\"scaleTitle\":\"W\",\"level\":20,\"icon\":\"energy\",\"title\":\"Fibar Group Power (2.0.49.4)\",\"isFailed\":true},\"order\":{\"rooms\":0,\"elements\":0,\"dashboard\":0},\"permanently_hidden\":false,\"probeType\":\"energy\",\"tags\":[],\"visibility\":true,\"updateTime\":1509385213}]},\"code\":200,\"message\":\"200 OK\", \"error\":null}");
                mock.Expect(HttpMethod.Get, "http://test/api/devices").WithHeaders("Cookie", data.CookieValue).WithExactQueryString("since=1509225434").Respond("application/json", "{\"data\":{\"structureChanged\":false,\"updateTime\":1509225435,\"devices\":[{\"creationTime\":1459276234,\"creatorId\":1,\"customIcons\":{},\"deviceType\":\"switchBinary\",\"h\":1078449915,\"hasHistory\":false,\"id\":\"ZWayVDev_zway_2-0-37\",\"location\":0,\"metrics\":{\"icon\":\"switch\",\"title\":\"Fibar Group (2.0) Switch\",\"level\":\"off\",\"isFailed\":true},\"order\":{\"rooms\":0,\"elements\":0,\"dashboard\":0},\"permanently_hidden\":false,\"probeType\":\"\",\"tags\":[],\"visibility\":true,\"updateTime\":1509385213}]},\"code\":200,\"message\":\"200 OK\", \"error\":null}");
                mock.When(HttpMethod.Get, "http://test/api/devices").WithHeaders("Cookie", data.CookieValue).WithExactQueryString("since=1509225435").Respond("application/json", "{\"data\":{\"structureChanged\":false,\"updateTime\":1509225435,\"devices\":[]},\"code\":200,\"message\":\"200 OK\", \"error\":null}");
            }

            return mock;
        }

        private class MockContainer
        {
            public Mock<ILogger<ZWayService>> Logger { get; set; }
            public MockHttpMessageHandler Http { get; set; }

            public ZWayService Service { get; set; }
        }
    }
}
