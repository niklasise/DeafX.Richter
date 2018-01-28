using DeafX.Richter.Business.Interfaces;
using DeafX.Richter.Business.Models;
using DeafX.Richter.Common.Extensions;

namespace DeafX.Richter.Web.Models
{
    public class DeviceViewModelCollection
    {
        public DeviceViewModel[] Devices { get; set; }

        public long LastUpdated { get; set; }
    }

    public class DeviceViewModel
    {
        public string id { get; set; }
        public string title { get; set; }
        public string valueType { get; set; }
        public object value { get; set; }
        public virtual string deviceType => "VALUE_DEVICE";
        public long lastChanged { get; set; }

        protected DeviceViewModel(IDevice device)
        {
            id = device.Id;
            title = device.Title;
            valueType = device.ValueType.ToString();
            value = device.Value;
            lastChanged = device.LastChanged.ToUnixTimeStamp();
        }

        public static DeviceViewModel FromDevice(IDevice device)
        {
            if (device is IToggleDevice)
            {
                return new ToggleDeviceViewModel(device as IToggleDevice);
            }
            else
            { 
                return new DeviceViewModel(device);
            }
        }
    }

    public class ToggleDeviceViewModel : DeviceViewModel
    {
        public bool automated { get; set; }

        public DeviceTimerViewModel timer { get; set; }

        public override string deviceType => "TOGGLE_DEVICE";

        public ToggleDeviceViewModel(IToggleDevice device) : base(device)
        {
            automated = device.Automated;
            timer = device.Timer == null ? null : DeviceTimerViewModel.FromToggleTimer(device.Timer);
        }
    }

    public class DeviceTimerViewModel
    {
        public int timerValue { get; set; }

        public bool stateToSet { get; set; }

        public static DeviceTimerViewModel FromToggleTimer(ToggleTimer timer)
        {
            return new DeviceTimerViewModel()
            {
                timerValue = timer.RemainingSeconds,
                stateToSet = timer.StateToToggle
            };
        }
    }

}
