using System;
using System.Threading.Tasks;

namespace DeafX.Richter.Business.Interfaces
{
    public delegate void OnDevicesUpdatedHandler(object sender, DevicesUpdatedEventArgs args);

    public interface IDeviceService
    {
        IDevice[] GetAllDevices();
        Task ToggleDeviceAsync(string deviceId, bool toggled);
        event OnDevicesUpdatedHandler OnDevicesUpdated;
    }

    public class DevicesUpdatedEventArgs : EventArgs
    {
        public IDevice[] UpdatedDevices { get; private set; }

        public DevicesUpdatedEventArgs(IDevice[] updateDevices)
        {
            UpdatedDevices = updateDevices;
        }
    }
}
