using DeafX.Richter.Business.Interfaces;
using DeafX.Richter.Business.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using System;
using DeafX.Richter.Business.Models;
using System.Collections.Generic;
using System.Linq;

namespace DeafX.Richter.Business.Test
{
    [TestClass]
    public class DeviceGroupServiceTest
    {

        private class MockData
        {
            public TestDevice[] AllSubDevices { get; set; } =
            {
                new TestDevice()
                {
                    Id = "TestDevice1",
                    Title = "Test Device #1"
                },
                new TestDevice()
                {
                    Id = "TestDevice2",
                    Title = "Test Device #2"
                }
            };
        }


        [TestMethod]
        public void Init()
        {
            var data = new MockData();
            var container = GetMockContainer(data);

            container.Service.Init(new DeviceGroupConfiguration[]
            {
                new DeviceGroupConfiguration()
                {
                    Id = "DeviceGroup1",
                    Title = "Device Group #1",
                    Devices = new string[] { "TestDevice1", "TestDevice2" }
                }
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InitFailIncorrectId()
        {
            var data = new MockData();
            var container = GetMockContainer(data);

            container.Service.Init(new DeviceGroupConfiguration[]
            {
                new DeviceGroupConfiguration()
                {
                    Id = "DeviceGroup1",
                    Title = "Device Group #1",
                    Devices = new string[] { "TestDevice1", "TestDevice3" }
                }
            });
        }

        [TestMethod]
        public async Task GetAllDevices()
        {
            var data = new MockData();
            var container = GetContainerAndInitService(data);

            var devices = container.Service.GetAllDevices();

            Assert.AreEqual(1, devices.Length);
            Assert.AreEqual("DeviceGroup1", devices[0].Id);
            Assert.AreEqual("Device Group #1", devices[0].Title);

            var deviceGroup = devices[0] as DeviceGroup;

            Assert.AreEqual(2, deviceGroup.Devices.Length);
            Assert.AreEqual("TestDevice1", deviceGroup.Devices[0].Id);
            Assert.AreEqual("TestDevice2", deviceGroup.Devices[1].Id);
            Assert.IsFalse(deviceGroup.Toggled);
        }

        [TestMethod]
        public async Task ToggleDevice()
        {
            var data = new MockData();
            var container = GetContainerAndInitService(data);

            var updateDevices = new List<IDevice[]>();

            container.Service.OnDevicesUpdated += (o, e) =>
            {
                updateDevices.Add(e.UpdatedDevices);
            };

            await container.Service.ToggleDeviceAsync("DeviceGroup1", true);

            Assert.AreEqual(1, updateDevices.Count);
            Assert.AreEqual(1, updateDevices[0].Length);

            var deviceGroup = updateDevices[0][0] as DeviceGroup;

            Assert.AreEqual("DeviceGroup1", deviceGroup.Id);
            Assert.IsTrue(deviceGroup.Toggled);
            Assert.IsTrue(deviceGroup.Devices[0].Toggled);
            Assert.IsTrue(deviceGroup.Devices[1].Toggled);
        }

        private MockContainer GetContainerAndInitService(MockData data)
        {
            var container = GetMockContainer(data);

            container.Service.Init(new DeviceGroupConfiguration[]
            {
                new DeviceGroupConfiguration()
                {
                    Id = "DeviceGroup1",
                    Title = "Device Group #1",
                    Devices = new string[] { "TestDevice1", "TestDevice2" }
                }
            });

            return container;
        }

        private Mock<IDeviceService> GetMockSubService(MockData data)
        {
            var mock = new Mock<IDeviceService>(MockBehavior.Strict);

            foreach(var device in data.AllSubDevices)
            {
                device.ParentService = mock.Object;
            }

            mock.Setup(m => m.GetAllDevices()).Returns(data.AllSubDevices);
            mock.Setup(m => m.ToggleDeviceAsync(It.IsAny<string>(), true)).
                Returns(
                    (string id, bool toggled) => 
                    {
                        var t = new Task(() => { data.AllSubDevices.First(d => d.Id == id).Toggled = toggled; });
                        t.Start();
                        return t;
                    });

            return mock;
        }

        private MockContainer GetMockContainer(MockData data)
        {
            var mockContainer = new MockContainer()
            {
                SubService = GetMockSubService(data)
            };

            mockContainer.Service = new DeviceGroupService(new IDeviceService[] { mockContainer.SubService.Object });

            return mockContainer;
        }

        private class MockContainer
        {
            public Mock<IDeviceService> SubService { get; set; }
            
            public DeviceGroupService Service { get; set; }
        }

        private class TestDevice : IToggleDeviceInternal
        {
            public bool Toggled { get; set; }

            public string Id { get; set; }

            public string Title { get; set; }

            public object Value => Toggled;

            public bool Automated { get; set; }

            public DeviceValueType ValueType => DeviceValueType.Toggle;

            public IDeviceService ParentService { get; set; }

            public event DeviceValueChangedHandler OnValueChanged;

            public ToggleTimer Timer { get; internal set; }

            ToggleTimer IToggleDevice.Timer
            {
                get { return Timer; }
            }

            ToggleTimer IToggleDeviceTimerSet.Timer
            {
                set { Timer = value; }
            }
        }
    }
}
