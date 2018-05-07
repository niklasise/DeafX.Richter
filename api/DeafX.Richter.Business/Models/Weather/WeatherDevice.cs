using DeafX.Richter.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeafX.Richter.Business.Models.Weather
{
    public abstract class WeatherDevice : IDevice
    {

        public string Id { get; private set; }

        public string Title { get; private set; }

        public object Value
        {
            get;
            protected set;
        }

        public abstract DeviceValueType ValueType { get; }

        public IDeviceService ParentService { get; private set; }

        public DateTime LastChanged { get; private set; }

        public event DeviceValueChangedHandler OnValueChanged;

        public WeatherDevice(string id, string title, IDeviceService parentService)
        {
            this.Id = id;
            this.Title = title;
            this.ParentService = parentService;
        }

        protected void InvokeValueChanged()
        {
            LastChanged = DateTime.Now;
            OnValueChanged?.Invoke(this);
        }


    }
}
