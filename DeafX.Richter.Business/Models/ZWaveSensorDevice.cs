using DeafX.Richter.Business.Exceptions;
using DeafX.Richter.Business.Interfaces;
using DeafX.Richter.Business.Models.ZWay;
using System;

namespace DeafX.Richter.Business.Models
{
    public class ZWaveSensorDevice : IDevice
    {
        public string Id { get; private set; }

        public IDeviceService ParentService { get; private set; }

        public object Value {
            get
            {
                return ValueType == DeviceValueType.Binary ? ParseBinaryLevel() : InternalDevice.metrics.level;
            }
        }

        public DeviceValueType ValueType {
            get
            {
                if (InternalDevice.deviceType.Equals("sensorbinary", StringComparison.InvariantCultureIgnoreCase))
                {
                    return DeviceValueType.Binary;
                }
                else
                {
                    switch (InternalDevice.probeType?.ToLower())
                    {
                        case "temperature":
                            return DeviceValueType.Temperature;
                        case "luminosity":
                            return DeviceValueType.Luminosity;
                    }
                }

                return DeviceValueType.Unknown;
            }
        }

        internal ZWayDevice InternalDevice { get; private set; }

        public ZWaveSensorDevice(string id, ZWayDevice zWayDevice , IDeviceService parentService)
        {
            Id = id;
            ParentService = parentService;
            InternalDevice = zWayDevice;

            if(InternalDevice.ParentDevice != null)
            {
                throw new ZWayDeviceConfigurationException("A ZWayDevice cannot have multiple parent devices");
            }

            if (!string.Equals(InternalDevice.deviceType, "sensormultilevel", StringComparison.InvariantCultureIgnoreCase) &&
                !string.Equals(InternalDevice.deviceType, "sensorbinary", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ZWayDeviceConfigurationException($"{nameof(ZWayDevice)} must have deviceType 'sensormultilevel' or 'sensorBinary'. Is '{InternalDevice.deviceType}'");
            }

            InternalDevice.ParentDevice = this;
        }

        private bool ParseBinaryLevel()
        {
            return string.Equals(InternalDevice.metrics.level?.ToString(), "on", StringComparison.OrdinalIgnoreCase);
        }
    }
}
