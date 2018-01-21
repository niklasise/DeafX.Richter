using System;
using System.Threading.Tasks;

namespace DeafX.Richter.Business.Interfaces
{
    public delegate void OnDevicesUpdatedHandler(object sender, DevicesUpdatedEventArgs args);

    public interface IDeviceService
    {
        IDevice[] GetAllDevices();
        IDevice[] GetUpdatedDevices(DateTime since);
        Task ToggleDeviceAsync(string deviceId, bool toggled);
        void SetAutomated(string deviceId, bool automated);
        void SetTimer(string deviceId, int seconds, bool stateToSet);
        void AbortTimer(string deviceId);
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
