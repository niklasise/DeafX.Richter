//using DeafX.Richter.Business.Interfaces;
//using DeafX.Richter.Business.Models.ZWay;
//using DeafX.Richter.Business.Services;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace DeafX.Richter.Business.Models
//{
//    public abstract class ZWaveDevice<T> : IDevice<T> where T : IComparable
//    {
//        protected ZWayDevice _internalDevice;
//        protected ZWayService _service;

//        public DateTime LastChanged
//        {
//            get; private set;
//        }

//        public T Value
//        {
//            get
//            {
//                return ParseLevel(_internalDevice.metrics.level);
//            }
//            set
//            {
//                SetValue(value);
//            }
//        }

//        public event ValueObjectChangedHandler<T> ValueChanged;

//        private T _value;

//        public ZWaveDevice(ZWayDevice zwayDevice, ZWayService service) 
//        {
//            _service = service;
//            _internalDevice = zwayDevice;
//            _internalDevice.DeviceUpdated += OnInternalDeviceUpdated;
//        }

//        private void OnInternalDeviceUpdated(object sender, ZWayDeviceUpdatedEventArgs args)
//        {
//            LastChanged = DateTime.Now;

//            ValueChanged?.Invoke(this, new ValueObjectChangedEventArgs<T>(Value, ParseLevel(args.NewMetrics.level)));
//        }

//        protected abstract T ParseLevel(object level);

//        protected abstract void SetValue(T value);
//    }
//}
