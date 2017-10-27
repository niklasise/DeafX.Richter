using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace DeafX.Richter.Business.Models.ZWay
{
    public delegate void ZWayDeviceUpdated(object sender, ZWayDeviceUpdatedEventArgs args);

    public class ZWayDevice
    { 
        public int creationTime { get; set; }
        public string deviceType { get; set; }
        public int h { get; set; }
        public bool hasHistory { get; set; }
        public string id { get; set; }
        public int location { get; set; }
        public bool permanently_hidden { get; set; }
        public string probeType { get; set; }
        public List<object> tags { get; set; }
        public bool visibility { get; set; }
        public int updateTime { get; set; }
        public int? creatorId { get; set; }
        public ZWayMetrics metrics { get; set; }

        public event ZWayDeviceUpdated DeviceUpdated;

        private ILogger<ZWayDevice> _logger;

        public ILogger<ZWayDevice> Logger
        {
            get
            {
                if(_logger == null)
                {
                    _logger = LoggerFactoryWrapper.CreateLogger<ZWayDevice>();
                }

                return _logger;
            }
        }

        public ZWayDevice()
        {
            
        }

        public void UpdateMetrics(ZWayMetrics metrics, int updateTime)
        {
            this.updateTime = updateTime;          

            // If metrics are equal, just return
            if (this.metrics.Equals(metrics))
            {
                return;
            }

            this.metrics = metrics;
            DeviceUpdated?.Invoke(this, new ZWayDeviceUpdatedEventArgs()
            {
                NewMetrics = metrics,
                UpdateTime = updateTime
            });
        }
    }

    public class ZWayDeviceUpdatedEventArgs : EventArgs
    {
        public int UpdateTime { get; set; }
        public ZWayMetrics NewMetrics { get; set; }
    }
}
