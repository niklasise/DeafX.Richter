using DeafX.Richter.Business.Interfaces;
using DeafX.Richter.Business.Models;
using DeafX.Richter.Business.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeafX.Richter.Business.Test
{
    [TestClass]
    public class AggregatedServiceTest
    {

        private class MockData
        {
            public TestDevice[] AllSubDevices { get; set; } =
            {
                new TestDevice()
                {
                    Id = "TestDevice1",
                    Title = "Test Device #1",
                    Value = 21
                },
                new TestToggleDevice()
                {
                    Id = "TestDevice2",
                    Title = "Test Device #2"
                }
            };

            public TriggerConfiguration[] TriggerConfigurations { get; set; } =
            {
                new TriggerConfiguration()
                {
                    Id = "Trigger1",
                    DeviceToToggle = "TestDevice2",
                    StateToSet = true,
                    Title = "Trigger #1",
                    Conditions = new ITriggerConditionConfiguration[]
                    {
                        new DeviceConditionConfiguration()
                        {
                            Device = "TestDevice1",
                            CompareOperator = DeviceConditionOperator.GreaterOrEqual,
                            CompareValue = 22
                        }
                    }
                }
            };
        }

        #region Init Tests

        [TestMethod]
        public void Init()
        {
            var data = new MockData();
            var container = GetMockContainer(data);

            container.Service.Init(data.TriggerConfigurations);

            Assert.AreEqual(1, container.Service.ToggleTriggers.Count());
            Assert.AreEqual("Trigger1", container.Service.ToggleTriggers.First().Id);
            Assert.AreEqual(data.AllSubDevices.First(d => d.Id == "TestDevice2"), container.Service.ToggleTriggers.First().DeviceToToggle);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InitFailIncorrectId()
        {
            var data = new MockData();
            var container = GetMockContainer(data);

            container.Service.Init(new TriggerConfiguration[]
            {
                new TriggerConfiguration() {
                    Id = "Trigger1",
                    DeviceToToggle = "TestDevice3",
                    StateToSet = true,
                    Title = "Trigger #1",
                    Conditions = new ITriggerConditionConfiguration[]
                    {
                            new DeviceConditionConfiguration()
                            {
                                Device = "TestDevice1",
                                CompareOperator = DeviceConditionOperator.GreaterOrEqual,
                                CompareValue = 22
                            }
                    }
                }
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InitFailIncorrectIdCondition()
        {
            var data = new MockData();
            var container = GetMockContainer(data);

            container.Service.Init(new TriggerConfiguration[]
            {
                new TriggerConfiguration() {
                    Id = "Trigger1",
                    DeviceToToggle = "TestDevice2",
                    StateToSet = true,
                    Title = "Trigger #1",
                    Conditions = new ITriggerConditionConfiguration[]
                    {
                            new DeviceConditionConfiguration()
                            {
                                Device = "TestDevice3",
                                CompareOperator = DeviceConditionOperator.GreaterOrEqual,
                                CompareValue = 22
                            }
                    }
                }
            });
        }

        #endregion

        #region DeviceTrigger Tests

        [TestMethod]
        public async Task DeviceTriggerSuccessTriggeredAtStart()
        {
            var data = new MockData();

            data.TriggerConfigurations[0].Conditions = new ITriggerConditionConfiguration[]
            {
                new DeviceConditionConfiguration()
                {
                    CompareOperator = DeviceConditionOperator.GreaterOrEqual,
                    CompareValue = 21,
                    Device = "TestDevice1"
                }
            };

            var container = GetMockContainer(data);

            var deviceToTrigger = data.AllSubDevices.First(d => d.Id == "TestDevice2") as TestToggleDevice;
            var deviceThatTriggers = data.AllSubDevices.First(d => d.Id == "TestDevice1");

            Assert.IsFalse(deviceToTrigger.Toggled);

            container.Service.Init(data.TriggerConfigurations);

            await Task.Delay(10);

            Assert.IsTrue(deviceToTrigger.Toggled);
        }

        [TestMethod]
        public async Task DeviceTriggerSuccessGreater()
        {
            var data = new MockData();

            data.TriggerConfigurations[0].Conditions = new ITriggerConditionConfiguration[]
            {
                new DeviceConditionConfiguration()
                {
                    CompareOperator = DeviceConditionOperator.Greater,
                    CompareValue = 22,
                    Device = "TestDevice1"
                }
            };

            var container = GetMockContainer(data);

            var deviceToTrigger = data.AllSubDevices.First(d => d.Id == "TestDevice2") as TestToggleDevice;
            var deviceThatTriggers = data.AllSubDevices.First(d => d.Id == "TestDevice1");

            container.Service.Init(data.TriggerConfigurations);

            Assert.IsFalse(deviceToTrigger.Toggled);

            deviceThatTriggers.Value = 22;

            await Task.Delay(10);

            Assert.IsFalse(deviceToTrigger.Toggled);

            deviceThatTriggers.Value = 23;

            await Task.Delay(10);

            Assert.IsTrue(deviceToTrigger.Toggled);
        }

        [TestMethod]
        public async Task DeviceTriggerSuccessGreaterOrEqual()
        {
            var data = new MockData();

            data.TriggerConfigurations[0].Conditions = new ITriggerConditionConfiguration[]
            {
                new DeviceConditionConfiguration()
                {
                    CompareOperator = DeviceConditionOperator.GreaterOrEqual,
                    CompareValue = 23,
                    Device = "TestDevice1"
                }
            };

            var container = GetMockContainer(data);

            var deviceToTrigger = data.AllSubDevices.First(d => d.Id == "TestDevice2") as TestToggleDevice;
            var deviceThatTriggers = data.AllSubDevices.First(d => d.Id == "TestDevice1");

            container.Service.Init(data.TriggerConfigurations);

            Assert.IsFalse(deviceToTrigger.Toggled);

            deviceThatTriggers.Value = 22;

            await Task.Delay(10);

            Assert.IsFalse(deviceToTrigger.Toggled);

            deviceThatTriggers.Value = 23;

            await Task.Delay(10);

            Assert.IsTrue(deviceToTrigger.Toggled);
        }

        [TestMethod]
        public async Task DeviceTriggerSuccessLessOrEqual()
        {
            var data = new MockData();

            data.TriggerConfigurations[0].Conditions = new ITriggerConditionConfiguration[]
            {
                new DeviceConditionConfiguration()
                {
                    CompareOperator = DeviceConditionOperator.LessOrEqual,
                    CompareValue = 19,
                    Device = "TestDevice1"
                }
            };

            var container = GetMockContainer(data);

            var deviceToTrigger = data.AllSubDevices.First(d => d.Id == "TestDevice2") as TestToggleDevice;
            var deviceThatTriggers = data.AllSubDevices.First(d => d.Id == "TestDevice1");

            container.Service.Init(data.TriggerConfigurations);

            Assert.IsFalse(deviceToTrigger.Toggled);

            deviceThatTriggers.Value = 20;

            await Task.Delay(10);

            Assert.IsFalse(deviceToTrigger.Toggled);

            deviceThatTriggers.Value = 19;

            await Task.Delay(10);

            Assert.IsTrue(deviceToTrigger.Toggled);
        }

        [TestMethod]
        public async Task DeviceTriggerSuccessLess()
        {
            var data = new MockData();

            data.TriggerConfigurations[0].Conditions = new ITriggerConditionConfiguration[]
            {
                new DeviceConditionConfiguration()
                {
                    CompareOperator = DeviceConditionOperator.Less,
                    CompareValue = 19,
                    Device = "TestDevice1"
                }
            };

            var container = GetMockContainer(data);

            var deviceToTrigger = data.AllSubDevices.First(d => d.Id == "TestDevice2") as TestToggleDevice;
            var deviceThatTriggers = data.AllSubDevices.First(d => d.Id == "TestDevice1");

            container.Service.Init(data.TriggerConfigurations);

            Assert.IsFalse(deviceToTrigger.Toggled);

            deviceThatTriggers.Value = 19;

            await Task.Delay(10);

            Assert.IsFalse(deviceToTrigger.Toggled);

            deviceThatTriggers.Value = 18;

            await Task.Delay(10);

            Assert.IsTrue(deviceToTrigger.Toggled);
        }

        [TestMethod]
        public async Task DeviceTriggerSuccessEqual()
        {
            var data = new MockData();

            data.TriggerConfigurations[0].Conditions = new ITriggerConditionConfiguration[]
            {
                new DeviceConditionConfiguration()
                {
                    CompareOperator = DeviceConditionOperator.Equal,
                    CompareValue = 22,
                    Device = "TestDevice1"
                }
            };

            var container = GetMockContainer(data);

            var deviceToTrigger = data.AllSubDevices.First(d => d.Id == "TestDevice2") as TestToggleDevice;
            var deviceThatTriggers = data.AllSubDevices.First(d => d.Id == "TestDevice1");

            container.Service.Init(data.TriggerConfigurations);

            Assert.IsFalse(deviceToTrigger.Toggled);

            deviceThatTriggers.Value = 20;

            await Task.Delay(10);

            Assert.IsFalse(deviceToTrigger.Toggled);

            deviceThatTriggers.Value = 23;

            await Task.Delay(10);

            Assert.IsFalse(deviceToTrigger.Toggled);

            deviceThatTriggers.Value = 22;

            await Task.Delay(10);

            Assert.IsTrue(deviceToTrigger.Toggled);
        }

        #endregion

        private MockContainer GetContainerAndInitService(MockData data)
        {
            var container = GetMockContainer(data);

            return container;
        }

        private Mock<IDeviceService> GetMockSubService(MockData data)
        {
            var mock = new Mock<IDeviceService>(MockBehavior.Strict);

            foreach (var device in data.AllSubDevices)
            {
                device.ParentService = mock.Object;
            }

            mock.Setup(m => m.GetAllDevices()).Returns(data.AllSubDevices);
            mock.Setup(m => m.ToggleDeviceAsync(It.IsAny<string>(), true)).
                Returns(
                    (string id, bool toggled) =>
                    {
                        var t = new Task(() => { (data.AllSubDevices.First(d => d.Id == id) as TestToggleDevice).Toggled = toggled; });
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

            mockContainer.Service = new AggregatedDeviceService(new IDeviceService[] { mockContainer.SubService.Object });

            return mockContainer;
        }

        private class MockContainer
        {
            public Mock<IDeviceService> SubService { get; set; }

            public AggregatedDeviceService Service { get; set; }
        }

        private class TestDevice : IDevice
        {
            private object _value;

            public string Id { get; set; }

            public string Title { get; set; }

            public virtual DeviceValueType ValueType { get; set; } = DeviceValueType.Temperature;

            public IDeviceService ParentService { get; set; }

            public event DeviceValueChangedHandler OnValueChanged;

            public virtual object Value {
                get
                {
                    return _value;
                }
                set
                {
                    _value = value;
                    OnValueChanged?.Invoke(this);
                }
            }
        }

        private class TestToggleDevice : TestDevice, IToggleDevice
        {
            public bool Toggled { get { return Value == null ? false : (bool)Value; } set { Value = value; } }

            public override DeviceValueType ValueType => DeviceValueType.Toggle;
        }
    }
}
