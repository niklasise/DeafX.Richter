using DeafX.Richter.Business.Interfaces;
using DeafX.Richter.Business.Models;
using DeafX.Richter.Business.Services;
using DeafX.Richter.Common.Logging;
using Microsoft.Extensions.Logging;
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
                    Title = "Test Device #2",
                    Automated = true,
                },
                new TestToggleDevice()
                {
                    Id = "TestDevice3",
                    Title = "Test Device #3",
                    Automated = true
                }
            };

            public List<ToggleAutomationRuleConfiguration> RuleConfigurations { get; set; } = new List<ToggleAutomationRuleConfiguration>()
            {
                new ToggleAutomationRuleConfiguration()
                {
                    Id = "Rule1",
                    DeviceToToggle = "TestDevice2",
                    DeviceCondition = new DeviceConditionConfiguration
                    {
                        Device = "TestDevice1",
                        CompareOperator = DeviceConditionOperator.GreaterOrEqual,
                        CompareValue = 22
                    }
                },
                new ToggleAutomationRuleConfiguration()
                {
                    Id = "Rule2",
                    DeviceToToggle = "TestDevice3",
                    TimerCondition = new TimerConditionConfiguration
                    {
                        Intervals = new TimerConditionIntervalConfiguration[]
                        {
                            new TimerConditionIntervalConfiguration()
                            {
                                Start = "12:00",
                                End = "13:00"
                            }
                        }
                    }
                }
            };

            public DateTime DateTimeToUse { get; set; }
        }

        #region Init Tests

        [TestMethod]
        public void Init()
        {
            var data = new MockData();
            var container = GetMockContainer(data);

            container.Service.Init(data.RuleConfigurations.ToArray());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InitFailDuplicatedDeviceRules()
        {
            var data = new MockData();
            var container = GetMockContainer(data);

            data.RuleConfigurations.Add(new ToggleAutomationRuleConfiguration()
            {
                Id = "Rule3",
                DeviceToToggle = "TestDevice2",
                DeviceCondition = new DeviceConditionConfiguration
                {
                    Device = "TestDevice1",
                    CompareOperator = DeviceConditionOperator.GreaterOrEqual,
                    CompareValue = 22
                }
            });

            container.Service.Init(data.RuleConfigurations.ToArray());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InitFailIncorrectDeviceId()
        {
            var data = new MockData();
            var container = GetMockContainer(data);

            data.RuleConfigurations.Add(new ToggleAutomationRuleConfiguration()
            {
                Id = "Rule3",
                DeviceToToggle = "TestDevice4",
                DeviceCondition = new DeviceConditionConfiguration
                {
                    Device = "TestDevice1",
                    CompareOperator = DeviceConditionOperator.GreaterOrEqual,
                    CompareValue = 22
                }
            });

            container.Service.Init(data.RuleConfigurations.ToArray());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InitFailIncorrectConditionDeviceId()
        {
            var data = new MockData();
            var container = GetMockContainer(data);

            data.RuleConfigurations.Add(new ToggleAutomationRuleConfiguration()
            {
                Id = "Rule3",
                DeviceToToggle = "TestDevice3",
                DeviceCondition = new DeviceConditionConfiguration
                {
                    Device = "TestDevice4",
                    CompareOperator = DeviceConditionOperator.GreaterOrEqual,
                    CompareValue = 22
                }
            });

            container.Service.Init(data.RuleConfigurations.ToArray());
        }

        #endregion

        #region GetDevices Tests 

        [TestMethod]
        public void GetAllDevices()
        {
            var data = new MockData();
            var container = GetMockContainer(data);

            container.Service.Init(data.RuleConfigurations.ToArray());

            var devices = container.Service.GetAllDevices();

            Assert.AreEqual(3, devices.Length);
        }

        [TestMethod]
        public async void GetUpdatedDevices()
        {
            var data = new MockData();
            var container = GetMockContainer(data);

            container.Service.Init(data.RuleConfigurations.ToArray());

            var since = DateTime.Now;

            await container.Service.ToggleDeviceAsync("TestDevice2", true);

            var updatedDevices = container.Service.GetUpdatedDevices(since);

            Assert.AreEqual(1, updatedDevices.Length);
            Assert.AreEqual("TestDevice2", updatedDevices[0].Id);
            Assert.AreEqual(true, updatedDevices[0].Value);
        }

        #endregion

        #region DeviceCondition Tests

        [TestMethod]
        public async Task DeviceTriggerSuccessNotAutomated()
        {
            var data = new MockData();

            data.RuleConfigurations = new List<ToggleAutomationRuleConfiguration>()
            {
                new ToggleAutomationRuleConfiguration()
                {
                    Id = "Rule1",
                    DeviceToToggle = "TestDevice2",
                    DeviceCondition = new DeviceConditionConfiguration
                    {
                        Device = "TestDevice1",
                        CompareOperator = DeviceConditionOperator.Greater,
                        CompareValue = 20
                    }
                }
            };

            var container = GetMockContainer(data);

            var deviceToTrigger = data.AllSubDevices.First(d => d.Id == "TestDevice2") as TestToggleDevice;
            var deviceThatTriggers = data.AllSubDevices.First(d => d.Id == "TestDevice1");

            deviceToTrigger.Automated = false;

            container.Service.Init(data.RuleConfigurations.ToArray());

            await Task.Delay(10);

            Assert.IsFalse(deviceToTrigger.Toggled);

            deviceThatTriggers.Value = 20;

            await Task.Delay(10);

            Assert.IsFalse(deviceToTrigger.Toggled);

            deviceThatTriggers.Value = 21;

            await Task.Delay(10);

            Assert.IsFalse(deviceToTrigger.Toggled);
        }

        [TestMethod]
        public async Task DeviceTriggerSuccessGreater()
        {
            var data = new MockData();

            data.RuleConfigurations = new List<ToggleAutomationRuleConfiguration>()
            {
                new ToggleAutomationRuleConfiguration()
                {
                    Id = "Rule1",
                    DeviceToToggle = "TestDevice2",
                    DeviceCondition = new DeviceConditionConfiguration
                    {
                        Device = "TestDevice1",
                        CompareOperator = DeviceConditionOperator.Greater,
                        CompareValue = 20
                    }
                }
            };

            var container = GetMockContainer(data);

            var deviceToTrigger = data.AllSubDevices.First(d => d.Id == "TestDevice2") as TestToggleDevice;
            var deviceThatTriggers = data.AllSubDevices.First(d => d.Id == "TestDevice1");

            container.Service.Init(data.RuleConfigurations.ToArray());

            await Task.Delay(10);

            Assert.IsTrue(deviceToTrigger.Toggled);

            deviceThatTriggers.Value = 20;

            await Task.Delay(10);

            Assert.IsFalse(deviceToTrigger.Toggled);

            deviceThatTriggers.Value = 21;

            await Task.Delay(10);

            Assert.IsTrue(deviceToTrigger.Toggled);
        }

        [TestMethod]
        public async Task DeviceTriggerSuccessGreaterOrEqual()
        {
            var data = new MockData();

            data.RuleConfigurations = new List<ToggleAutomationRuleConfiguration>()
            {
                new ToggleAutomationRuleConfiguration()
                {
                    Id = "Rule1",
                    DeviceToToggle = "TestDevice2",
                    DeviceCondition = new DeviceConditionConfiguration
                    {
                        Device = "TestDevice1",
                        CompareOperator = DeviceConditionOperator.GreaterOrEqual,
                        CompareValue = 21
                    }
                }
            };

            var container = GetMockContainer(data);

            var deviceToTrigger = data.AllSubDevices.First(d => d.Id == "TestDevice2") as TestToggleDevice;
            var deviceThatTriggers = data.AllSubDevices.First(d => d.Id == "TestDevice1");

            container.Service.Init(data.RuleConfigurations.ToArray());

            await Task.Delay(10);

            Assert.IsTrue(deviceToTrigger.Toggled);

            deviceThatTriggers.Value = 20;

            await Task.Delay(10);

            Assert.IsFalse(deviceToTrigger.Toggled);

            deviceThatTriggers.Value = 21;

            await Task.Delay(10);

            Assert.IsTrue(deviceToTrigger.Toggled);
        }

        [TestMethod]
        public async Task DeviceTriggerSuccessEqual()
        {
            var data = new MockData();

            data.RuleConfigurations = new List<ToggleAutomationRuleConfiguration>()
            {
                new ToggleAutomationRuleConfiguration()
                {
                    Id = "Rule1",
                    DeviceToToggle = "TestDevice2",
                    DeviceCondition = new DeviceConditionConfiguration
                    {
                        Device = "TestDevice1",
                        CompareOperator = DeviceConditionOperator.Equal,
                        CompareValue = 21
                    }
                }
            };

            var container = GetMockContainer(data);

            var deviceToTrigger = data.AllSubDevices.First(d => d.Id == "TestDevice2") as TestToggleDevice;
            var deviceThatTriggers = data.AllSubDevices.First(d => d.Id == "TestDevice1");

            container.Service.Init(data.RuleConfigurations.ToArray());

            await Task.Delay(10);

            Assert.IsTrue(deviceToTrigger.Toggled);

            deviceThatTriggers.Value = 20;

            await Task.Delay(10);

            Assert.IsFalse(deviceToTrigger.Toggled);

            deviceThatTriggers.Value = 21;

            await Task.Delay(10);

            Assert.IsTrue(deviceToTrigger.Toggled);
        }

        [TestMethod]
        public async Task DeviceTriggerSuccessLess()
        {
            var data = new MockData();

            data.RuleConfigurations = new List<ToggleAutomationRuleConfiguration>()
            {
                new ToggleAutomationRuleConfiguration()
                {
                    Id = "Rule1",
                    DeviceToToggle = "TestDevice2",
                    DeviceCondition = new DeviceConditionConfiguration
                    {
                        Device = "TestDevice1",
                        CompareOperator = DeviceConditionOperator.Less,
                        CompareValue = 22
                    }
                }
            };

            var container = GetMockContainer(data);

            var deviceToTrigger = data.AllSubDevices.First(d => d.Id == "TestDevice2") as TestToggleDevice;
            var deviceThatTriggers = data.AllSubDevices.First(d => d.Id == "TestDevice1");

            container.Service.Init(data.RuleConfigurations.ToArray());

            await Task.Delay(10);

            Assert.IsTrue(deviceToTrigger.Toggled);

            deviceThatTriggers.Value = 22;

            await Task.Delay(10);

            Assert.IsFalse(deviceToTrigger.Toggled);

            deviceThatTriggers.Value = 21;

            await Task.Delay(10);

            Assert.IsTrue(deviceToTrigger.Toggled);
        }

        [TestMethod]
        public async Task DeviceTriggerSuccessLessOrEqual()
        {
            var data = new MockData();

            data.RuleConfigurations = new List<ToggleAutomationRuleConfiguration>()
            {
                new ToggleAutomationRuleConfiguration()
                {
                    Id = "Rule1",
                    DeviceToToggle = "TestDevice2",
                    DeviceCondition = new DeviceConditionConfiguration
                    {
                        Device = "TestDevice1",
                        CompareOperator = DeviceConditionOperator.LessOrEqual,
                        CompareValue = 21
                    }
                }
            };

            var container = GetMockContainer(data);

            var deviceToTrigger = data.AllSubDevices.First(d => d.Id == "TestDevice2") as TestToggleDevice;
            var deviceThatTriggers = data.AllSubDevices.First(d => d.Id == "TestDevice1");

            container.Service.Init(data.RuleConfigurations.ToArray());

            await Task.Delay(10);

            Assert.IsTrue(deviceToTrigger.Toggled);

            deviceThatTriggers.Value = 22;

            await Task.Delay(10);

            Assert.IsFalse(deviceToTrigger.Toggled);

            deviceThatTriggers.Value = 21;

            await Task.Delay(10);

            Assert.IsTrue(deviceToTrigger.Toggled);
        }

        #endregion

        #region TimerCondition Tests

        [TestMethod]
        public async Task TimerConditionSuccess()
        {
            LoggerFactoryWrapper.LoggerFactory = new LoggerFactory();
            //LoggerFactoryWrapper.LoggerFactory.AddFile("Logs/myapp-{Date}.txt", LogLevel.Debug);
            //LoggerFactoryWrapper.LoggerFactory.AddDebug(LogLevel.Debug);
            var logger = LoggerFactoryWrapper.CreateLogger<AggregatedServiceTest>();
            //LoggerFactoryWrapper.LoggerFactory.AddConsole(LogLevel.Debug);

            var data = new MockData();

            var startTime = DateTime.Now.TimeOfDay;

            data.RuleConfigurations = new List<ToggleAutomationRuleConfiguration>()
            {
                new ToggleAutomationRuleConfiguration()
                {
                    Id = "Rule1",
                    DeviceToToggle = "TestDevice2",
                    TimerCondition = new TimerConditionConfiguration()
                    {
                        Intervals = new TimerConditionIntervalConfiguration[]
                        {
                            new TimerConditionIntervalConfiguration()
                            {
                                Start = startTime.Add(TimeSpan.FromMilliseconds(300)).ToString(),
                                End = startTime.Add(TimeSpan.FromMilliseconds(800)).ToString()
                            }
                        }
                    }
                }
            };

            var container = GetMockContainer(data);

            var deviceToTrigger = data.AllSubDevices.First(d => d.Id == "TestDevice2") as TestToggleDevice;

            container.Service.Init(data.RuleConfigurations.ToArray());

            await Task.Delay(10);

            logger.LogDebug($"First assert (False) - RealTime - {DateTime.Now.TimeOfDay}");
            Assert.IsFalse(deviceToTrigger.Toggled);

            await Task.Delay(startTime + TimeSpan.FromMilliseconds(650) - DateTime.Now.TimeOfDay);

            logger.LogDebug($"Second assert (True) - RealTime - {DateTime.Now.TimeOfDay}");
            Assert.IsTrue(deviceToTrigger.Toggled);

            await Task.Delay(startTime + TimeSpan.FromMilliseconds(1150) - DateTime.Now.TimeOfDay);

            logger.LogDebug($"Third assert (False) - RealTime - {DateTime.Now.TimeOfDay}");
            Assert.IsFalse(deviceToTrigger.Toggled);
        }

        [TestMethod]
        public async Task TimerConditionSuccessAdditionalConditions()
        {
            var data = new MockData();

            var startTime = DateTime.Now.TimeOfDay;

            data.RuleConfigurations = new List<ToggleAutomationRuleConfiguration>()
            {
                new ToggleAutomationRuleConfiguration()
                {
                    Id = "Rule1",
                    DeviceToToggle = "TestDevice2",
                    TimerCondition = new TimerConditionConfiguration()
                    {
                        Intervals = new TimerConditionIntervalConfiguration[]
                        {
                            new TimerConditionIntervalConfiguration()
                            {
                                Start = startTime.Add(TimeSpan.FromMilliseconds(300)).ToString(),
                                End = startTime.Add(TimeSpan.FromMilliseconds(800)).ToString(),
                                AdditionalConditions = new DeviceConditionConfiguration[]
                                {
                                    new DeviceConditionConfiguration()
                                    {
                                        Device = "TestDevice1",
                                        CompareOperator = DeviceConditionOperator.Greater,
                                        CompareValue = 22
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var container = GetMockContainer(data);

            var deviceToTrigger = data.AllSubDevices.First(d => d.Id == "TestDevice2") as TestToggleDevice;
            var deviceThatTriggers = data.AllSubDevices.First(d => d.Id == "TestDevice1") as TestDevice;

            container.Service.Init(data.RuleConfigurations.ToArray());

            await Task.Delay(10);

            Assert.IsFalse(deviceToTrigger.Toggled);

            await Task.Delay(startTime + TimeSpan.FromMilliseconds(650) - DateTime.Now.TimeOfDay);

            Assert.IsFalse(deviceToTrigger.Toggled);

            deviceThatTriggers.Value = 23;

            await Task.Delay(10);

            Assert.IsTrue(deviceToTrigger.Toggled);

            deviceThatTriggers.Value = 21;

            await Task.Delay(10);

            Assert.IsFalse(deviceToTrigger.Toggled);

            deviceThatTriggers.Value = 23;

            await Task.Delay(10);

            Assert.IsTrue(deviceToTrigger.Toggled);

            await Task.Delay(startTime + TimeSpan.FromMilliseconds(1150) - DateTime.Now.TimeOfDay);

            Assert.IsFalse(deviceToTrigger.Toggled);
        }

        [TestMethod]
        public async Task TimerConditionSuccessTrueAtStart()
        {
            var data = new MockData();

            var startTime = DateTime.Now.TimeOfDay;

            data.RuleConfigurations = new List<ToggleAutomationRuleConfiguration>()
            {
                new ToggleAutomationRuleConfiguration()
                {
                    Id = "Rule1",
                    DeviceToToggle = "TestDevice2",
                    TimerCondition = new TimerConditionConfiguration()
                    {
                        Intervals = new TimerConditionIntervalConfiguration[]
                        {
                            new TimerConditionIntervalConfiguration()
                            {
                                Start = startTime.Add(TimeSpan.FromMilliseconds(-100)).ToString(),
                                End = startTime.Add(TimeSpan.FromMilliseconds(300)).ToString()
                            }
                        }
                    }
                }
            };

            var container = GetMockContainer(data);

            var deviceToTrigger = data.AllSubDevices.First(d => d.Id == "TestDevice2") as TestToggleDevice;

            container.Service.Init(data.RuleConfigurations.ToArray());

            await Task.Delay(10);

            Assert.IsTrue(deviceToTrigger.Toggled);

            await Task.Delay(startTime + TimeSpan.FromMilliseconds(650) - DateTime.Now.TimeOfDay);

            Assert.IsFalse(deviceToTrigger.Toggled);

        }

        [TestMethod]
        public async Task TimerConditionSuccessMultipleIntervals()
        {
            var data = new MockData();

            var startTime = DateTime.Now.TimeOfDay;

            data.RuleConfigurations = new List<ToggleAutomationRuleConfiguration>()
            {
                new ToggleAutomationRuleConfiguration()
                {
                    Id = "Rule1",
                    DeviceToToggle = "TestDevice2",
                    TimerCondition = new TimerConditionConfiguration()
                    {
                        Intervals = new TimerConditionIntervalConfiguration[]
                        {
                            new TimerConditionIntervalConfiguration()
                            {
                                Start = startTime.Add(TimeSpan.FromMilliseconds(300)).ToString(),
                                End = startTime.Add(TimeSpan.FromMilliseconds(800)).ToString()
                            },
                            new TimerConditionIntervalConfiguration()
                            {
                                Start = startTime.Add(TimeSpan.FromMilliseconds(1200)).ToString(),
                                End = startTime.Add(TimeSpan.FromMilliseconds(1700)).ToString()
                            }
                        }
                    }
                }
            };

            var container = GetMockContainer(data);

            var deviceToTrigger = data.AllSubDevices.First(d => d.Id == "TestDevice2") as TestToggleDevice;

            container.Service.Init(data.RuleConfigurations.ToArray());

            await Task.Delay(10);

            Assert.IsFalse(deviceToTrigger.Toggled);

            await Task.Delay(startTime + TimeSpan.FromMilliseconds(650) - DateTime.Now.TimeOfDay);

            Assert.IsTrue(deviceToTrigger.Toggled);

            await Task.Delay(startTime + TimeSpan.FromMilliseconds(1150) - DateTime.Now.TimeOfDay);

            Assert.IsFalse(deviceToTrigger.Toggled);

            await Task.Delay(startTime + TimeSpan.FromMilliseconds(1550) - DateTime.Now.TimeOfDay);

            Assert.IsTrue(deviceToTrigger.Toggled);

            await Task.Delay(startTime + TimeSpan.FromMilliseconds(2050) - DateTime.Now.TimeOfDay);

            Assert.IsFalse(deviceToTrigger.Toggled);
        }

        [TestMethod]
        public async Task TimerConditionSuccessMultipleIntervalsStartInMiddle()
        {
            var data = new MockData();

            var startTime = DateTime.Now.TimeOfDay;

            data.RuleConfigurations = new List<ToggleAutomationRuleConfiguration>()
            {
                new ToggleAutomationRuleConfiguration()
                {
                    Id = "Rule1",
                    DeviceToToggle = "TestDevice2",
                    TimerCondition = new TimerConditionConfiguration()
                    {
                        Intervals = new TimerConditionIntervalConfiguration[]
                        {
                            new TimerConditionIntervalConfiguration()
                            {
                                Start = startTime.Add(TimeSpan.FromMilliseconds(-2000)).ToString(),
                                End = startTime.Add(TimeSpan.FromMilliseconds(-1000)).ToString()
                            },
                            new TimerConditionIntervalConfiguration()
                            {
                                Start = startTime.Add(TimeSpan.FromMilliseconds(300)).ToString(),
                                End = startTime.Add(TimeSpan.FromMilliseconds(800)).ToString()
                            }
                        }
                    }
                }
            };

            var container = GetMockContainer(data);

            var deviceToTrigger = data.AllSubDevices.First(d => d.Id == "TestDevice2") as TestToggleDevice;

            container.Service.Init(data.RuleConfigurations.ToArray());

            await Task.Delay(10);

            Assert.IsFalse(deviceToTrigger.Toggled);

            await Task.Delay(startTime + TimeSpan.FromMilliseconds(650) - DateTime.Now.TimeOfDay);

            Assert.IsTrue(deviceToTrigger.Toggled);

            await Task.Delay(startTime + TimeSpan.FromMilliseconds(1150) - DateTime.Now.TimeOfDay);

            Assert.IsFalse(deviceToTrigger.Toggled);

        } 


        #endregion

        #region ToggleTimer Tests

        [TestMethod]
        public async Task ToggleTimerSuccess()
        {
            var data = new MockData();

            var container = GetMockContainer(data);

            var deviceToTrigger = data.AllSubDevices.First(d => d.Id == "TestDevice2") as TestToggleDevice;

            container.Service.Init(data.RuleConfigurations.ToArray());

            await Task.Delay(10);

            Assert.IsFalse(deviceToTrigger.Toggled);

            container.Service.SetTimer("TestDevice2", 1, true);

            Assert.IsFalse(deviceToTrigger.Toggled);

            await Task.Delay(1010);

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
            mock.Setup(m => m.ToggleDeviceAsync(It.IsAny<string>(), It.IsAny<bool>())).
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
                SubService = GetMockSubService(data),
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

            public DateTime LastChanged { get; set; }

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

        private class TestToggleDevice : TestDevice, IToggleDeviceInternal
        {
            public bool Toggled { get { return Value == null ? false : (bool)Value; } set { Value = value; } }

            public bool Automated { get; set; }

            public override DeviceValueType ValueType => DeviceValueType.Toggle;

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
