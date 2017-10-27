using DeafX.Richter.Business.Interfaces;
using DeafX.Richter.Business.Models.ZWay;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeafX.Richter.Business.Models
{
    public class ZWaveToggleDevice : IToggleDevice
    {
        public bool Toggled
        {
            get
            {
                if (InternalDevice.metrics.level == null)
                {
                    return false;
                }

                return InternalDevice.metrics.level.ToString().Equals("on", StringComparison.OrdinalIgnoreCase);
            }
        }

        public string Id => InternalDevice.id;

        public int LastChanged => InternalDevice.updateTime;

        public object Value => Toggled;

        public IDeviceService ParentService => throw new NotImplementedException();

        internal ZWayDevice InternalDevice
        {
            get; private set;
        }

        public ZWaveToggleDevice(ZWayDevice device)
        {
            InternalDevice = device;
        }
    }
}
