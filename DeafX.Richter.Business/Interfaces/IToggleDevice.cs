using System;
using System.Collections.Generic;
using System.Text;

namespace DeafX.Richter.Business.Interfaces
{
    public interface IToggleDevice : IDevice
    {
        bool Toggled { get; }

        bool Automated { get; }
    }
}
