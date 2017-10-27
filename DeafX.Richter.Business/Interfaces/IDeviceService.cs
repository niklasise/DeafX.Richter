using System.Threading.Tasks;

namespace DeafX.Richter.Business.Interfaces
{
    public interface IDeviceService
    {
        IDevice[] GetAllDevices();
        IDevice[] GetUpdatedDevices(int since);
        Task ToggleDeviceAsync(string deviceId, bool toggled);
    }
}
