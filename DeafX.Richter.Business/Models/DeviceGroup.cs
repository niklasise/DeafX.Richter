using DeafX.Richter.Business.Interfaces;
using System;

namespace DeafX.Richter.Business.Models
{
    public class DeviceGroup : IToggleDeviceInternal
    {
        private bool _toggled;

        public string Id
        {
            get; private set;
        }

        public object Value => Toggled;

        public IDeviceService ParentService
        {
            get; private set;
        }

        public bool Toggled
        {
            get
            {
                return _toggled;
            }

            set
            {
                if(value != _toggled)
                {
                    _toggled = value;
                    OnValueChanged?.Invoke(this);
                }
            }
        }

        public DateTime LastChanged { get; internal set; }

        DateTime IDevice.LastChanged
        {
            get { return LastChanged; }
        }

        DateTime IDeviceLastChangedSet.LastChanged
        {
            set { LastChanged = value; }
        }

        public string Title { get; private set; }

        public IToggleDevice[] Devices { get; private set; }

        public bool Automated { get; internal set; }

        public DeviceValueType ValueType => DeviceValueType.GroupToggle;

        public ToggleTimer Timer { get; internal set; }

        ToggleTimer IToggleDevice.Timer
        {
            get { return Timer; }
        }

        ToggleTimer IToggleDeviceTimerSet.Timer
        {
            set { Timer = value; }
        }

        public DeviceGroup(string id, string title, bool automated,IToggleDevice[] devices, IDeviceService parentService)
        {
            Id = id;
            ParentService = parentService;
            Devices = devices;
            Title = title;
            Automated = automated;
        }

        public event DeviceValueChangedHandler OnValueChanged;
    }
}
