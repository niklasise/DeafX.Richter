using System;
using System.Collections.Generic;
using System.Text;

namespace DeafX.Richter.Business.Models
{
    public class ToggleAutomationRuleConfiguration
    {

        public string Id { get; set; }

        //public string Title { get; set; }

        public string DeviceToToggle { get; set; }

        public IToggleAutomationConditionConfiguration Condition { get; set; }

        //public bool StateToSet { get; set; }

    }

    public interface IToggleAutomationConditionConfiguration { }

    public class TimerConditionConfiguration : IToggleAutomationConditionConfiguration
    {
        public TimerConditionIntervalConfiguration[] Intervals { get; set; }

        //public IToggleAutomationConditionConfiguration[] AdditionalConditions
        //public TimeSpan Interval { get; set; }
    }

    public class DeviceConditionConfiguration : IToggleAutomationConditionConfiguration
    {
        public string Device { get; set; }

        public IComparable CompareValue { get; set; }

        public DeviceConditionOperator CompareOperator { get; set; }
    }

    public class TimerConditionIntervalConfiguration
    {
        public TimeSpan Start { get; set; }

        public TimeSpan End { get; set; }

        public DeviceConditionConfiguration[] AdditionalConditions { get; set; }
    }
}
