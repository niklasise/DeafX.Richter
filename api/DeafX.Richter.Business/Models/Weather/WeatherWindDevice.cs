using DeafX.Richter.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeafX.Richter.Business.Models.Weather
{
    public class WeatherWindDevice : WeatherDevice
    {
        public double MaxValue { get; private set; }

        public double Direction { get; private set; }

        public string DirectionTextual { get; private set; }

        public override DeviceValueType ValueType => DeviceValueType.Wind;

        public WeatherWindDevice(string id, string title, IDeviceService parentService)
            : base(id, title, parentService) { }

        internal void SetValue(object value, double maxValue, double direction, string directionTextual)
        {
            bool changed = false;

            if ((Value == null && value != null) ||
                (Value != null && !Value.Equals(value)))
            {
                Value = value;
                changed = true;
            }

            if (!MaxValue.Equals(maxValue))
            {
                MaxValue = maxValue;
                changed = true;
            }

            if ((DirectionTextual == null && directionTextual != null) ||
                (DirectionTextual != null && !DirectionTextual.Equals(directionTextual)))
            {
                DirectionTextual = directionTextual;
                changed = true;
            }

            if (!Direction.Equals(direction))
            {
                Direction = direction;
                changed = true;
            }

            if (changed)
            {
                InvokeValueChanged();
            }
        }
    }
}
