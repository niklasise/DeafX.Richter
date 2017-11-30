using DeafX.Richter.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeafX.Richter.Web.Models
{
    public class DeviceViewModel
    {
        public string id { get; set; }
        public string title { get; set; }
        public string valueType { get; set; }
        public object value { get; set; }
        public virtual string deviceType => "VALUE_DEVICE";

        protected DeviceViewModel(IDevice device)
        {
            id = device.Id;
            title = device.Title;
            valueType = device.ValueType.ToString();
            value = device.Value;
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

        public int timerValue { get; set; }

        public override string deviceType => "TOGGLE_DEVICE";

        public ToggleDeviceViewModel(IToggleDevice device) : base(device)
        {
            automated = device.Automated;
            timerValue = device.Timer == null ? 0 : device.Timer.RemainingSeconds;
        }
    }

}
