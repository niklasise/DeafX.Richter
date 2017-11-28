using DeafX.Richter.Business.Exceptions;
using DeafX.Richter.Business.Interfaces;
using DeafX.Richter.Business.Models.ZWay;
using System;

namespace DeafX.Richter.Business.Models
{
    public class ZWavePowerPlugDevice : ZWaveDevice, IToggleDevice
    {
        internal ZWayDevice InternalPowerDevice { get; private set; }

        public bool Toggled => (bool)Value;

        public bool Automated { get; internal set; }

        public override DeviceValueType ValueType => DeviceValueType.Toggle;

        public string Power
        {
            get
            {
                return InternalPowerDevice.metrics?.level?.ToString();
            }
        }

        public ZWavePowerPlugDevice(string id, string title, bool automated, ZWayDevice switchDevice, ZWayDevice powerDevice, IDeviceService parentService)
            : base(id, title, switchDevice, parentService)
        {
            InternalPowerDevice = powerDevice;

            if (!string.Equals(InternalPowerDevice.deviceType, "sensormultilevel", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ZWayDeviceConfigurationException($"{nameof(ZWayDevice)} must have deviceType 'sensormultilevel'. Is '{InternalPowerDevice.deviceType}'");
            }

            if (!string.Equals(switchDevice.deviceType, "switchBinary", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ZWayDeviceConfigurationException($"{nameof(ZWayDevice)} must have deviceType 'switchBinary'. Is '{switchDevice.deviceType}'");
            }

            InternalPowerDevice.ParentDevice = this;
            Automated = automated;
        }
    }
}
