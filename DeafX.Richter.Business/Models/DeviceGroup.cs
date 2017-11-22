using DeafX.Richter.Business.Interfaces;

namespace DeafX.Richter.Business.Models
{
    public class DeviceGroup : IToggleDevice
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

        public int LastChanged
        {
            get; internal set;
        }

        public string Title { get; private set; }

        public IToggleDevice[] Devices { get; private set; }

        public DeviceValueType ValueType => DeviceValueType.GroupToggle;

        public DeviceGroup(string id, string title,IToggleDevice[] devices, IDeviceService parentService)
        {
            Id = id;
            ParentService = parentService;
            Devices = devices;
            Title = title;
        }

        public event DeviceValueChangedHandler OnValueChanged;
    }
}
