//using System.Threading;

//namespace DeafX.Richter.Business.Models
//{
//    public class TimerDevice : Device<Time>
//    {
//        private Timer _timer;

//        public TimerDevice() : base("SYS_TIMER")
//        {
//            _timer = new Timer(TimerElapsed, null, 0, 5000);
//        }

//        private void TimerElapsed(object state)
//        {
//            Value = Time.Now;
//        }
//    }
//}
