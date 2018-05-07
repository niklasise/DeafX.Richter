using DeafX.Richter.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeafX.Richter.Business.Models.Weather
{
    public class WeatherAirDevice : WeatherDevice
    {
        public double RealtiveHumidity { get; private set; }

        public override DeviceValueType ValueType =>  DeviceValueType.Temperature;

        public WeatherAirDevice(string id, string title, IDeviceService parentService)
            : base(id, title, parentService) { }

        internal void SetValue(object value, double relativeHumidity)
        {
            bool changed = false;

            if ((Value == null && value != null) ||
                (Value != null && !Value.Equals(value)))
            {
                Value = value;
                changed = true;
            }

            if (!RealtiveHumidity.Equals(relativeHumidity))
            {
                RealtiveHumidity = relativeHumidity;
                changed = true;
            }

            if (changed)
            {
                InvokeValueChanged();
            }
        }

    }
}
