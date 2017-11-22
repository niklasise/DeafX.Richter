using DeafX.Richter.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DeafX.Richter.Business.Models
{
    public class TimerCondition : ITriggerCondition
    {
        public TimeSpan TriggerTime { get; private set; }

        //public TimeSpan Interval { get; private set; }

        public bool Fullfilled { get; private set; }

        public TimerCondition(TimeSpan time/*, TimeSpan interval*/)
        {
            TriggerTime = time;
            //Interval = interval;
            StartTimer();
        }

        //public TimerCondition(TimeSpan time) : this(time, TimeSpan.FromDays(1)) { }
        
        public event OnTriggerConditionFullfilledHandler OnConditionFullfilled;

        public void Reset()
        {
            Fullfilled = false;
            StartTimer();
        }

        private async void StartTimer()
        {
            // Calculate time left until timer should be triggered
            var timeOfDay = DateTime.Now.TimeOfDay;

            var timeLeft = timeOfDay >= TriggerTime ? 
                TriggerTime + TimeSpan.FromHours(24) - timeOfDay : 
                timeOfDay - TriggerTime;

            await Task.Delay(timeLeft);

            Fullfilled = true;
            OnConditionFullfilled?.Invoke(this);
        }
    }
}
