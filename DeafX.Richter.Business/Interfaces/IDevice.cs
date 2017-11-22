using System;

namespace DeafX.Richter.Business.Interfaces
{
    //public interface IDevice<T>
    //{
    //    DateTime LastChanged { get; }

    //    T Value { get; set; }

    //    event ValueObjectChangedHandler<T> ValueChanged;
    //}

    //public interface IDevice<T>
    //{
    //    DateTime LastChanged { get; }

    //    T Value { get; set; }
    //}

    //public interface IDevice : IDevice<object> { }

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

    //public class DeviceValueChangedEventArgs : EventArgs
    //{
    //    public DeviceValueChangedEventArgs(object previousValue, object newValue)
    //    {
    //        PreviousValue = previousValue;
    //        NewValue = newValue;
    //    }

    //    public object PreviousValue { get; private set; }
    //    public object NewValue { get; private set; }
    //}
}
