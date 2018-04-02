using DeafX.Richter.Business.Interfaces;
using DeafX.Richter.Common.DataStorage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeafX.Richter.Business.Services
{
    public class StatisticsService
    {
        private ILogger<StatisticsService> _logger;
        private IDataOverTimeStorage _dataStorage;
        private IDeviceService _deviceService;
        private StatisticsServiceConfiguration _configuration;

        public StatisticsService(ILogger<StatisticsService> logger, IDeviceService deviceService, IDataOverTimeStorage dataStorage, StatisticsServiceConfiguration configuration)
        {
            _logger = logger;
            _dataStorage = dataStorage;
            _deviceService = deviceService;
            _configuration = configuration;
        }

        public StatisticsService(ILogger<StatisticsService> logger, IDeviceService deviceService, StatisticsServiceConfiguration configuration)
            : this(logger, deviceService, new LiteDbDataStorage(configuration.StoragePath), configuration) { }

        public void Init()
        {
            CaptureDevices();
        }

        public IEnumerable<DataTimeObject<double>> GetStatistics(string deviceId)
        {
            return GetStatistics(deviceId, DateTime.MinValue, DateTime.MaxValue, TimeSpan.Zero);
        }

        public IEnumerable<DataTimeObject<double>> GetStatistics(string deviceId, DateTime from)
        {
            return GetStatistics(deviceId, from, DateTime.MaxValue, TimeSpan.Zero);
        }

        public IEnumerable<DataTimeObject<double>> GetStatistics(string deviceId, DateTime from, DateTime to)
        {
            return GetStatistics(deviceId, from, to, TimeSpan.Zero);
        }

        public IEnumerable<DataTimeObject<double>> GetStatistics(string deviceId, DateTime from, DateTime to, TimeSpan minimumDataInterval)
        {
            var orginalData = _dataStorage.Retreive<double>(deviceId, from, to).ToList();

            if(orginalData == null)
            {
                return null;
            }

            if(orginalData.Count < 2)
            {
                return orginalData;
            }

            orginalData.Sort();

            var meanData = new List<DataTimeObject<double>>();
            
            var intervalList = new List<DataTimeObject<double>>() { orginalData[0] };

            for(var i = 1; i < orginalData.Count ; i++)
            {
                if(orginalData[i].DateTime - intervalList[0].DateTime >= minimumDataInterval)
                {
                    meanData.Add(GetMeanDataPoint(intervalList));
                    intervalList = new List<DataTimeObject<double>>() { orginalData[i] };
                }
                else
                {
                    intervalList.Add(orginalData[i]);
                }

                // If last item in the list, calculate mean value of interval list and add to meanData
                if (i + 1 == orginalData.Count)
                {
                    meanData.Add(GetMeanDataPoint(intervalList));
                }
            }

            return meanData;
        }

        private async void CaptureDevices()
        {
            for (; ; )
            {
                var devices = _deviceService.GetAllDevices();

                foreach (var deviceId in _configuration.DevicesToCapture)
                {
                    var device = devices.FirstOrDefault(d => d.Id == deviceId);

                    if (device != null)
                    {
                        _dataStorage.Store(deviceId, Convert.ToDouble(device.Value));
                    }
                }

                await Task.Delay(_configuration.CaptureInterval);
            }
        }

        private DataTimeObject<double> GetMeanDataPoint(List<DataTimeObject<double>> dataPoints)
        {
            if(dataPoints.Count == 0)
            {
                throw new ArgumentException("Cant calculate mean valúe of an empty list");
            }

            if (dataPoints.Count == 1)
            {
                return dataPoints[0];
            }

            var firstDateTime = dataPoints.First().DateTime;
            var averageDateTime = firstDateTime.AddSeconds(dataPoints.Average(d => (d.DateTime - firstDateTime).TotalSeconds));

            return new DataTimeObject<double>()
            {
                DateTime = averageDateTime,
                Data = dataPoints.Average(d => d.Data)
            };
        }
    }

    public class StatisticsServiceConfiguration
    {
        public IEnumerable<string> DevicesToCapture { get; set; }

        public int CaptureInterval { get; set; }

        public string StoragePath { get; set; }
    }
}
