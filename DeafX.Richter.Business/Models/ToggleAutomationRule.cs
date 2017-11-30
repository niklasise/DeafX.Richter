using DeafX.Richter.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeafX.Richter.Business.Models
{
    public delegate void OnToggleAutomationRuleStateChangedHandler(object sender, ToggleAutomationRuleStateChangedHandler args);
    
    public class ToggleAutomationRule
    {
        public string Id { get; set; }

        public bool State { get; private set; }

        public IToggleDevice ToggleDevice { get; private set; }

        public IToggleAutomationCondition Condition { get; set; }

        public event OnToggleAutomationRuleStateChangedHandler OnStateChanged;

        public ToggleAutomationRule(string id, IToggleDevice toggleDevice, IToggleAutomationCondition condition)
        {
            Id = id;
            ToggleDevice = toggleDevice;
            Condition = condition;
            State = Condition.State;

            Condition.OnStateChanged += Condition_OnStateChanged;
        }

        private void Condition_OnStateChanged(object sender, ToggleAutomationConditionStateChangedHandler args)
        {
            if(args.NewState != State)
            {
                State = args.NewState;
                OnStateChanged.Invoke(this, new ToggleAutomationRuleStateChangedHandler(this, args.NewState));
            }
        }
    }
    
    public class ToggleAutomationRuleStateChangedHandler : EventArgs
    {
        public ToggleAutomationRule Rule { get; private set; }

        public bool NewState { get; private set; }

        public ToggleAutomationRuleStateChangedHandler(ToggleAutomationRule rule, bool newState)
        {
            Rule = rule;
            NewState = newState;
        }
    }
}
