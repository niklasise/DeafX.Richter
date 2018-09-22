using System;
using System.Collections.Generic;
using System.Text;

namespace DeafX.Richter.Business.Models
{
    public interface IToggleAutomationConditionConfiguration { }

    public class ToggleAutomationRuleConfiguration
    {

        public string Id { get; set; }

        public string DeviceToToggle { get; set; }

        public DeviceConditionConfiguration DeviceCondition { get; set; }

        public TimerConditionConfiguration TimerCondition { get; set; }

    }

    public class DeviceConditionConfiguration : IToggleAutomationConditionConfiguration
    {
        public string Device { get; set; }

        public int CompareValue { get; set; }

        public DeviceConditionOperator CompareOperator { get; set; }
    }

    public class TimerConditionConfiguration : IToggleAutomationConditionConfiguration
    {
        public TimerConditionIntervalConfiguration[] Intervals { get; set; }
    }

    public class TimerConditionIntervalConfiguration
    {
        public string Start { get; set; }

        public string End { get; set; }

        public DeviceConditionConfiguration[] AdditionalConditions { get; set; }
    }
}
