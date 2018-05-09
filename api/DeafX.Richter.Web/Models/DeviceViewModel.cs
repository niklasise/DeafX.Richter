using DeafX.Richter.Business.Interfaces;
using DeafX.Richter.Business.Models;
using DeafX.Richter.Business.Models.Weather;
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
            if(device is WeatherAirDevice)
            {
                return new WeatherAirDeviceViewModel(device as WeatherAirDevice);
            }
            if (device is WeatherPercipitationDevice)
            {
                return new WeatherPercipitationDeviceViewModel(device as WeatherPercipitationDevice);
            }
            if (device is WeatherWindDevice)
            {
                return new WeatherWindDeviceViewModel(device as WeatherWindDevice);
            }
            else if (device is IToggleDevice)
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

        public string powerConsumption { get; set; }

        public ToggleDeviceViewModel(IToggleDevice device) : base(device)
        {
            automated = device.Automated;
            powerConsumption = device.PowerConsumption;
            timer = device.Timer == null ? null : DeviceTimerViewModel.FromToggleTimer(device.Timer);
        }
    }

    public class WeatherAirDeviceViewModel : DeviceViewModel
    {
        public double relativeHumidity { get; set; }

        public override string deviceType => "WEATHER_AIR_DEVICE";

        public WeatherAirDeviceViewModel(WeatherAirDevice device) : base(device)
        {
            relativeHumidity = device.RealtiveHumidity;
        }
    }

    public class WeatherPercipitationDeviceViewModel : DeviceViewModel
    {
        public string amountTextual { get; set; }

        public string type { get; set; }

        public override string deviceType => "WEATHER_PERCIP_DEVICE";

        public WeatherPercipitationDeviceViewModel(WeatherPercipitationDevice device) : base(device)
        {
            amountTextual = device.AmountTextual;
            type = device.Type;
        }
    }

    public class WeatherWindDeviceViewModel : DeviceViewModel
    {
        public double maxValue { get; set; }

        public double direction { get; set; }

        public string directionTextual { get; set; }

        public override string deviceType => "WEATHER_WIND_DEVICE";

        public WeatherWindDeviceViewModel(WeatherWindDevice device) : base(device)
        {
            maxValue = device.MaxValue;
            direction = device.Direction;
            directionTextual = device.DirectionTextual;
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
