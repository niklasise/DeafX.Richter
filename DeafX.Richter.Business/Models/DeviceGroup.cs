using DeafX.Richter.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeafX.Richter.Business.Models
{
    public class DeviceGroup : IToggleDevice
    {
        public string Id
        {
            get; private set;
        }

        public object Value => Toggled;

        public IDeviceService ParentService
        {
            get; private set;
        }

        public bool Toggled
        {
            get; internal set;
        }

        public int LastChanged
        {
            get; internal set;
        }

        public IToggleDevice[] Devices { get; private set; }

        public DeviceGroup(string id, IToggleDevice[] devices, IDeviceService parentService)
        {
            Id = id;
            ParentService = parentService;
            Devices = devices;
        }

    }
}
