using DeafX.Richter.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeafX.Richter.Business.Models
{
    public class ToggleTrigger
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public bool StateToSet { get; private set; }

        public IToggleDevice DeviceToToggle { get; private set; }

        public ITriggerCondition[] TriggerConditions { get; private set; }

        public event OnTriggeredHandler OnTriggered;

        public bool Triggered { get; private set; }

        public ToggleTrigger(string id, string title, bool stateToSet, IToggleDevice deviceToToggle, ITriggerCondition[] conditions)
        {
            Id = id;
            Title = title;
            StateToSet = stateToSet;
            DeviceToToggle = deviceToToggle;
            TriggerConditions = conditions;

            foreach(var condition in TriggerConditions)
            {
                condition.OnConditionFullfilled += (sender) => EvalutateConditions();
            }

            EvalutateConditions();
        }

        private void EvalutateConditions()
        {
            if(Triggered)
            {
                return;
            }

            if(TriggerConditions.All(c => c.Fullfilled))
            {
                Triggered = true;
                OnTriggered?.Invoke(this);
            }
        }
    
        public void Reset()
        {
            Triggered = false;

            foreach (var condition in TriggerConditions)
            {
                condition.Reset();
            }
        }

    }

    public delegate void OnTriggeredHandler(object sender);
}
