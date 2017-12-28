using DeafX.Richter.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeafX.Richter.Business.Interfaces
{
    public interface IToggleDevice : IDevice
    {
        bool Toggled { get; }

        bool Automated { get; }

        ToggleTimer Timer { get; }
    }

    internal interface IToggleDeviceTimerSet
    {
        ToggleTimer Timer { set; }
    }

    internal interface IToggleDeviceInternal : IToggleDevice, IDeviceInternal, IToggleDeviceTimerSet { }
}
