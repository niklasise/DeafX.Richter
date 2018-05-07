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

        DateTime LastChanged { get; }

        event DeviceValueChangedHandler OnValueChanged;
    }

    internal interface IDeviceLastChangedSet
    {
        DateTime LastChanged { set; }
    }

    internal interface IDeviceInternal : IDevice, IDeviceLastChangedSet { }

    public enum DeviceValueType
    {
        Unknown,
        Binary,
        Toggle,
        GroupToggle,
        Temperature,
        Luminosity,
        Percipitation,
        Wind
    }

    public delegate void DeviceValueChangedHandler(object sender);

}
