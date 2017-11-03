using DeafX.Richter.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeafX.Richter.Web.Models
{
    public abstract class DeviceViewModel
    {
        public string id { get; set; }
        public string title { get; set; }
        public abstract string deviceType { get; }

        protected DeviceViewModel(IDevice device)
        {
            id = device.Id;
            title = device.Title;
        }

        public static DeviceViewModel FromDevice(IDevice device)
        {
            switch(device.ValueType)
            {
                case DeviceValueType.Binary:
                    return new ToggleDeviceViewModel(device);
                default:
                    return new ValueDeviceViewModel(device);
            }
        }
    }

    public class ToggleDeviceViewModel : DeviceViewModel
    {
        public bool toggled { get; set; }

        public override string deviceType => "TOGGLE_DEVICE";

        public ToggleDeviceViewModel(IDevice device) : base(device)
        {
            toggled = (bool)device.Value;
        }
    }

    public class ValueDeviceViewModel : DeviceViewModel
    {
        public override string deviceType => "VALUE_DEVICE";

        public string value { get; set; }
        public string valueType { get; set; }

        public ValueDeviceViewModel(IDevice device) : base(device)
        {
            value = device.Value?.ToString();
            valueType = device.ValueType.ToString();
        }
    }
}
