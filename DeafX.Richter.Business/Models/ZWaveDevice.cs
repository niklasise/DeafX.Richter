using DeafX.Richter.Business.Exceptions;
using DeafX.Richter.Business.Interfaces;
using DeafX.Richter.Business.Models.ZWay;
using System;

namespace DeafX.Richter.Business.Models
{
    public class ZWaveDevice : IDeviceInternal
    {
        public string Id { get; private set; }

        public string Title { get; private set; }

        public IDeviceService ParentService { get; private set; }

        public DateTime LastChanged { get; internal set; }

        DateTime IDevice.LastChanged
        {
            get { return LastChanged; }
        }

        DateTime IDeviceLastChangedSet.LastChanged
        {
            set { LastChanged = value; }
        }

        public object Value {
            get
            {
                return ValueType == DeviceValueType.Binary || ValueType == DeviceValueType.Toggle ? 
                    ParseBinaryLevel() : InternalDevice.metrics.level;
            }
        }

        public virtual DeviceValueType ValueType {
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

        public event DeviceValueChangedHandler OnValueChanged;

        internal ZWayDevice InternalDevice { get; private set; }

        public ZWaveDevice(string id, string title, ZWayDevice zWayDevice , IDeviceService parentService)
        {
            Id = id;
            Title = title;
            ParentService = parentService;
            InternalDevice = zWayDevice;

            if(InternalDevice.ParentDevice != null)
            {
                throw new ZWayDeviceConfigurationException("A ZWayDevice cannot have multiple parent devices");
            }

            if (!string.Equals(InternalDevice.deviceType, "sensormultilevel", StringComparison.InvariantCultureIgnoreCase) &&
                !string.Equals(InternalDevice.deviceType, "sensorbinary", StringComparison.InvariantCultureIgnoreCase) &&
                !string.Equals(InternalDevice.deviceType, "switchBinary", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ZWayDeviceConfigurationException($"{nameof(ZWayDevice)} must have deviceType 'sensormultilevel', 'sensorBinary' or 'switchBinary'. Is '{InternalDevice.deviceType}'");
            }

            InternalDevice.ParentDevice = this;
            InternalDevice.OnDeviceUpdated += OnInternalDeviceUpdated;
        }

        private void OnInternalDeviceUpdated(object sender)
        {
            OnValueChanged?.Invoke(this);
        }

        private bool ParseBinaryLevel()
        {
            return string.Equals(InternalDevice.metrics.level?.ToString(), "on", StringComparison.OrdinalIgnoreCase);
        }
    }
}
