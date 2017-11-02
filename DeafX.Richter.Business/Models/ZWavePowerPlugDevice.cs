﻿using DeafX.Richter.Business.Exceptions;
using DeafX.Richter.Business.Interfaces;
using DeafX.Richter.Business.Models.ZWay;
using System;

namespace DeafX.Richter.Business.Models
{
    public class ZWavePowerPlugDevice : IToggleDevice
    {
        internal ZWayDevice InternalSwitchDevice { get; private set; }

        internal ZWayDevice InternalPowerDevice { get; private set; }

        public bool Toggled => ParseLevel();

        public string Id { get; private set; }

        public IDeviceService ParentService { get; private set; }

        public object Value => Toggled;

        public DeviceValueType ValueType => DeviceValueType.Binary;

        public string Power {
            get
            {
                return InternalPowerDevice.metrics?.level?.ToString();
            }
        }

        public ZWavePowerPlugDevice(string id, ZWayDevice switchDevice, ZWayDevice powerDevice, IDeviceService parentService)
        {
            Id = id;
            ParentService = parentService;
            InternalSwitchDevice = switchDevice;
            InternalPowerDevice = powerDevice;

            if (InternalSwitchDevice.ParentDevice != null || InternalPowerDevice.ParentDevice != null)
            {
                throw new ZWayDeviceConfigurationException("A ZWayDevice cannot have multiple parent devices");
            }

            if (!string.Equals(InternalPowerDevice.deviceType, "sensormultilevel", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ZWayDeviceConfigurationException($"{nameof(ZWayDevice)} must have deviceType 'sensormultilevel'. Is '{InternalPowerDevice.deviceType}'");
            }

            if (!string.Equals(InternalSwitchDevice.deviceType, "switchBinary", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ZWayDeviceConfigurationException($"{nameof(ZWayDevice)} must have deviceType 'switchBinary'. Is '{InternalSwitchDevice.deviceType}'");
            }

            InternalPowerDevice.ParentDevice = this;
            InternalSwitchDevice.ParentDevice = this;
        }

        private bool ParseLevel()
        {
            return string.Equals(InternalSwitchDevice.metrics.level?.ToString(), "on", StringComparison.OrdinalIgnoreCase);
        }
    }
}