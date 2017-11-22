using DeafX.Richter.Business.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace DeafX.Richter.Business.Models.ZWay
{
    internal delegate void ZWayDeviceUpdatedHandler(object sender);

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

        internal IDevice ParentDevice { get; set; }

        internal event ZWayDeviceUpdatedHandler OnDeviceUpdated;

        internal bool UpdateMetrics(ZWayMetrics metrics)
        {
            // If metrics are equal, just return
            if (this.metrics.Equals(metrics))
            {
                return false;
            }

            this.metrics = metrics;

            OnDeviceUpdated?.Invoke(this);

            return true;
        }
    }

}
