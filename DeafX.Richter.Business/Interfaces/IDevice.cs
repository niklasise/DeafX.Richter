using System;

namespace DeafX.Richter.Business.Interfaces
{
    public interface IDevice
    {
        string Id { get; }

        string Title { get; }

        object Value { get; }

        DeviceValueType ValueType { get; } 

        IDeviceService ParentService { get; }

        event DeviceValueChangedHandler OnValueChanged;
    }

    public enum DeviceValueType
    {
        Unknown,
        Binary,
        Toggle,
        GroupToggle,
        Temperature,
        Luminosity
    }

    public delegate void DeviceValueChangedHandler(object sender);

}
