using DeafX.Richter.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeafX.Richter.Business.Models.Weather
{
    public class WeatherPercipitationDevice : WeatherDevice
    {
        public string AmountTextual { get; private set; }

        public string Type { get; private set; }

        public override DeviceValueType ValueType => DeviceValueType.Percipitation;

        public WeatherPercipitationDevice(string id, string title, IDeviceService parentService)
            : base(id, title, parentService) { }


        internal void SetValue(object value, string amountTextual, string type)
        {
            bool changed = false;

            if ((Value == null && value != null) ||
                (Value != null && !Value.Equals(value)))
            { 
                Value = value;
                changed = true;
            }

            if ((AmountTextual == null && amountTextual != null) ||
                (AmountTextual != null && !AmountTextual.Equals(amountTextual)))
            {
                AmountTextual = amountTextual;
                changed = true;
            }

            if ((Type == null && type != null) ||
                (Type != null && !Type.Equals(type)))
            {
                Type = type;
                changed = true;
            }

            if (changed)
            {
                InvokeValueChanged();
            }
        }

    }
}
