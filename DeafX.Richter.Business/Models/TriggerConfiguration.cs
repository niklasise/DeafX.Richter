using System;
using System.Collections.Generic;
using System.Text;

namespace DeafX.Richter.Business.Models
{
    public class TriggerConfiguration
    {

        public string Id { get; set; }

        public string Title { get; set; }

        public string DeviceToToggle { get; set; }

        public ITriggerConditionConfiguration[] Conditions { get; set; }

        public bool StateToSet { get; set; }

    }

    public interface ITriggerConditionConfiguration { }

    public class TimeConditionConfiguration : ITriggerConditionConfiguration
    {
        public TimeSpan Time { get; set; }

        //public TimeSpan Interval { get; set; }
    }

    public class DeviceConditionConfiguration : ITriggerConditionConfiguration
    {
        public string Device { get; set; }

        public IComparable CompareValue { get; set; }

        public DeviceConditionOperator CompareOperator { get; set; }
    }   
}
