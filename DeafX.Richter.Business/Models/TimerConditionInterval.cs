using DeafX.Richter.Business.Interfaces;
using System;

namespace DeafX.Richter.Business.Models
{
    public class TimerConditionInterval : IComparable<TimerConditionInterval>
    {
        public TimeSpan Start { get; private set; }

        public TimeSpan End { get; private set; }

        public IToggleAutomationCondition[] AdditionalConditions { get; set; }

        public TimerConditionInterval(TimeSpan start, TimeSpan end, IToggleAutomationCondition[] additionalConditions)
        {
            if (start == end)
            {
                throw new ArgumentException("End-time must be different than Start-time");
            }

            if (start.Days != 0 || end.Days != 0)
            {
                throw new ArgumentException("Start or End cannot have time assigned with days");
            }

            Start = start;
            End = end;
            AdditionalConditions = additionalConditions;
        }

        public int CompareTo(TimerConditionInterval other)
        {
            return Start.CompareTo(other.Start);
        }
    }
}
