//using DeafX.Richter.Business.Models.ZWay;
//using DeafX.Richter.Business.Services;
//using System;

//namespace DeafX.Richter.Business.Models
//{
//    public class ZWaveSwitchDevice : ZWaveDevice<bool>
//    {
//        public ZWaveSwitchDevice(ZWayDevice zwayDevice, ZWayService service)
//            : base(zwayDevice, service)
//        {

//        }

//        protected override bool ParseLevel(object level)
//        {
//            if (level == null)
//            {
//                return false;
//            }

//            return level.ToString().Equals("on", StringComparison.OrdinalIgnoreCase);
//        }

//        protected override void SetValue(bool value)
//        {
//            _service.ToggleDeviceAsync(_internalDevice, value).Wait();
//        }
//    }
//}
