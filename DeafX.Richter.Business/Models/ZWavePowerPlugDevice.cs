using DeafX.Richter.Business.Exceptions;
using DeafX.Richter.Business.Interfaces;
using DeafX.Richter.Business.Models.ZWay;
using System;

namespace DeafX.Richter.Business.Models
{
    public class ZWavePowerPlugDevice : ZWaveDevice, IToggleDevice, IToggleDeviceInternal
    {
        internal ZWayDevice InternalPowerDevice { get; private set; }

        public bool Toggled => (bool)Value;

        public bool Automated { get; internal set; }

        public override DeviceValueType ValueType => DeviceValueType.Toggle;

        public string PowerConsumption
        {
            get
            {
                return InternalPowerDevice?.metrics?.level?.ToString();
            }
        }

        public ToggleTimer Timer { get; internal set; }

        ToggleTimer IToggleDevice.Timer
        {
            get { return Timer; }
        }

        ToggleTimer IToggleDeviceTimerSet.Timer
        {
            set { Timer = value; }
        }

        public ZWavePowerPlugDevice(string id, string title, bool automated, ZWayDevice switchDevice, ZWayDevice powerDevice, IDeviceService parentService)
            : base(id, title, switchDevice, parentService)
        {
            InternalPowerDevice = powerDevice;

            if (InternalPowerDevice != null && !string.Equals(InternalPowerDevice.deviceType, "sensormultilevel", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ZWayDeviceConfigurationException($"{nameof(ZWayDevice)} must have deviceType 'sensormultilevel'. Is '{InternalPowerDevice.deviceType}'");
            }

            if (!string.Equals(switchDevice.deviceType, "switchBinary", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ZWayDeviceConfigurationException($"{nameof(ZWayDevice)} must have deviceType 'switchBinary'. Is '{switchDevice.deviceType}'");
            }

            if (InternalPowerDevice != null)
            {
                InternalPowerDevice.ParentDevice = this;
            }

            Automated = automated;
        }
    }
}
