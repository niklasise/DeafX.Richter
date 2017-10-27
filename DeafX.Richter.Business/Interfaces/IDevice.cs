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

        int LastChanged { get; }

        object Value { get; }

        IDeviceService ParentService { get; }
    }
}
