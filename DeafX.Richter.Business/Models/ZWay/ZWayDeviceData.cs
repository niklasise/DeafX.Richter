using System.Collections.Generic;

namespace DeafX.Richter.Business.Models.ZWay
{
    public class ZWayDeviceData
    {
        public bool structureChanged { get; set; }
        public int updateTime { get; set; }
        public List<ZWayDevice> devices { get; set; }
    }
}
