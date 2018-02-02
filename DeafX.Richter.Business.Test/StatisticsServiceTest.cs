using DeafX.Richter.Business.Interfaces;
using DeafX.Richter.Business.Services;
using DeafX.Richter.Common.DataStorage;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeafX.Richter.Business.Test
{
    [TestClass]
    public class StatisticsServiceTest
    {

        private class MockData
        {
            public StatisticsServiceConfiguration Configuration { get; set; } = new StatisticsServiceConfiguration()
            {
                DevicesToCapture = new string[] { "TestDevice1" },
                CaptureInterval = 500,
            };

            public TestDevice[] AllSubDevices { get; set; } =
            {
                new TestDevice()
                {
                    Id = "TestDevice1",
                    Title = "Test Device #1",
                    Value = 21.5
                },
            };

            public List<DataTimeObject<double>> StoredData { get; set; } = new List<DataTimeObject<double>>()
            {
                new DataTimeObject<double>()
                {
                    DateTime = new DateTime(2016,08,28,12,00,00),
                    Data = 12.5
                },
                new DataTimeObject<double>()
                {
                    DateTime = new DateTime(2016,08,28,13,00,00),
                    Data = 13
                },
                new DataTimeObject<double>()
                {
                    DateTime = new DateTime(2016,08,28,14,00,00),
                    Data = 14
                },
                new DataTimeObject<double>()
                {
                    DateTime = new DateTime(2016,08,28,14,10,00),
                    Data = 14.10
                },
                new DataTimeObject<double>()
                {
                    DateTime = new DateTime(2016,08,28,14,20,00),
                    Data = 14.20
                },
                new DataTimeObject<double>()
                {
                    DateTime = new DateTime(2016,08,28,15,00,00),
                    Data = 15
                },
                new DataTimeObject<double>()
                {
                    DateTime = new DateTime(2016,08,28,20,00,00),
                    Data = 20
                },
                new DataTimeObject<double>()
                {
                    DateTime = new DateTime(2016,08,28,20,00,01),
                    Data = 21.2
                },
            };
        }

        [TestMethod]
        public async Task Init()
        {
            var data = new MockData();
            var container = GetMockContainer(data);

            data.StoredData.Clear();

            container.Service.Init();

            var result = container.Service.GetStatistics("TestDevice1").ToArray();

            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(21.5, result[0].Data);

            data.AllSubDevices[0].Value = 22.5;

            await Task.Delay(800);

            result = container.Service.GetStatistics("TestDevice1").ToArray();

            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(22.5, result[1].Data);
        }

        [TestMethod]
        public void GetStatistics()
        {
            var data = new MockData();
            var container = GetMockContainer(data);

            var result = container.Service.GetStatistics(
                "TestDevice1",
                new DateTime(2016, 08, 28, 13, 00, 00),
                new DateTime(2016, 08, 28, 20, 00, 00),
                TimeSpan.FromHours(1)).ToArray();

            Assert.AreEqual(4, result.Length);

            Assert.AreEqual(new DateTime(2016, 08, 28, 13, 00, 00), result[0].DateTime);
            Assert.AreEqual(13, result[0].Data);

            Assert.AreEqual(new DateTime(2016, 08, 28, 14, 10, 00), result[1].DateTime);
            Assert.AreEqual(14.10, result[1].Data);

            Assert.AreEqual(new DateTime(2016, 08, 28, 15, 00, 00), result[2].DateTime);
            Assert.AreEqual(15, result[2].Data);

            Assert.AreEqual(new DateTime(2016, 08, 28, 20, 00, 00), result[3].DateTime);
            Assert.AreEqual(20, result[3].Data);
        }

        private MockContainer GetContainerAndInitService(MockData data)
        {
            var container = GetMockContainer(data);

            container.Service.Init();
            
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

            return mock;
        }

        private MockContainer GetMockContainer(MockData data)
        {
            var mockContainer = new MockContainer()
            {
                DeviceService = GetMockSubService(data),
                Logger = new Mock<ILogger<StatisticsService>>(),
                DataStorage = GetMockDataStorage(data)
            };

            mockContainer.Service = new StatisticsService(
                logger: mockContainer.Logger.Object,
                deviceService: mockContainer.DeviceService.Object,
                configuration: data.Configuration,
                dataStorage: mockContainer.DataStorage.Object       
            );

            return mockContainer;
        }

        private Mock<IDataOverTimeStorage> GetMockDataStorage(MockData data)
        {
            var mock = new Mock<IDataOverTimeStorage>(MockBehavior.Strict);

            mock.Setup(m => m.Store<double>(It.IsAny<string>(), It.IsAny<double>()))
                .Callback<string, double>(
                    (name, datas) => data.StoredData.Add(new DataTimeObject<double>()
                       {
                           DateTime = DateTime.Now,
                           Data = datas
                       })
                );

            mock.Setup(m => m.Retreive<double>(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns<string, DateTime, DateTime>(
                    (name, from, to) => data.StoredData.Where(d => d.DateTime >= from && d.DateTime <= to).ToArray()
                );

            return mock;
        }

        private class MockContainer
        {
            public Mock<ILogger<StatisticsService>> Logger { get; set; }

            public Mock<IDeviceService> DeviceService { get; set; }

            public Mock<IDataOverTimeStorage> DataStorage { get; set; }

            public StatisticsService Service { get; set; }
        }

        private class TestDevice : IDeviceInternal
        {
            public string Id { get; set; }

            public string Title { get; set; }

            public object Value { get; set; }

            public bool Automated { get; set; }

            public DateTime LastChanged { get; set; }

            public DeviceValueType ValueType => DeviceValueType.Toggle;

            public IDeviceService ParentService { get; set; }

            public event DeviceValueChangedHandler OnValueChanged;
        }
    }
}
