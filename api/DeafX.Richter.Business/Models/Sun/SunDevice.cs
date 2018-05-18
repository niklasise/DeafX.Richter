using DeafX.Richter.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeafX.Richter.Business.Models.Sun
{
    public class SunDevice : IDevice
    {
        public string Id { get; private set; }

        public string Title { get; private set; }

        public object Value => SunHours;

        public DeviceValueType ValueType => DeviceValueType.Sun;

        public IDeviceService ParentService { get; private set; }

        public DateTime LastChanged { get; private set; }

        public int SunHours { get; set; }

        public DateTime SunRise { get; private set; }

        public DateTime SunSet { get; private set; }

        public event DeviceValueChangedHandler OnValueChanged;

        public SunDevice(string id, string title, IDeviceService parentService)
        {
            this.Id = id;
            this.Title = title;
            this.ParentService = parentService;
        }

        public void SetValues(int sunHours, DateTime sunRise, DateTime sunSet)
        {
            SunHours = sunHours;
            SunRise = sunRise;
            SunSet = sunSet;

            LastChanged = DateTime.Now;
            OnValueChanged?.Invoke(this);
        }
    }
}
