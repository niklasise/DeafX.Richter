using System;
using System.Collections.Generic;
using System.Text;

namespace DeafX.Richter.Business.Interfaces
{
    public delegate void OnToggleAutomationConditionStateChangedHandler(object sender, ToggleAutomationConditionStateChangedHandler args);

    public interface IToggleAutomationCondition
    {
        event OnToggleAutomationConditionStateChangedHandler OnStateChanged;
        bool State { get; }
    }

    public class ToggleAutomationConditionStateChangedHandler : EventArgs
    {
        public IToggleAutomationCondition Condition { get; private set; }

        public bool NewState { get; private set; }

        public ToggleAutomationConditionStateChangedHandler(IToggleAutomationCondition condition, bool newState)
        {
            Condition = condition;
            NewState = newState;
        }
    }
}
