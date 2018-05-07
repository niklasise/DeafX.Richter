using DeafX.Richter.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeafX.Richter.Business.Models.Weather
{
    public class WeatherRoadDevice : WeatherDevice
    {

        public override DeviceValueType ValueType =>  DeviceValueType.Temperature;

        public WeatherRoadDevice(string id, string title, IDeviceService parentService)
            : base(id, title, parentService) { }

        internal void SetValue(object value)
        {
            if ((Value == null && value != null) ||
                (Value != null && !Value.Equals(value)))
            {
                Value = value;
                InvokeValueChanged();
            }
        }

    }
}
