using DeafX.Richter.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeafX.Richter.Business.Models
{
    public enum DeviceConditionOperator
    {
        Equal,
        Less,
        LessOrEqual,
        Greater,
        GreaterOrEqual
    }

    public class DeviceCondition : IToggleAutomationCondition
    {
        private IComparable _compareValue;
        private DeviceConditionOperator _compareOperator;
        private IDevice _device;

        public bool State { get; private set; }

        public event OnToggleAutomationConditionStateChangedHandler OnStateChanged;

        public DeviceCondition(IDevice device, IComparable compareValue, DeviceConditionOperator compareOperator)
        {
            _device = device;
            _compareOperator = compareOperator;
            _compareValue = compareValue;

            device.OnValueChanged += (sender) => CalculateState();

            CalculateState();
        }
        
        private void CalculateState()
        {
            var newState = CompareWithOperator(_compareValue.CompareTo(_device.Value));

            if (State != newState)
            {
                State = newState;
                OnStateChanged?.Invoke(this, new ToggleAutomationConditionStateChangedHandler(this, newState));
            }
        }

        private bool CompareWithOperator(int compareValue)
        {
            switch (_compareOperator)
            {
                case DeviceConditionOperator.Greater:
                    return compareValue < 0;
                case DeviceConditionOperator.GreaterOrEqual:
                    return compareValue <= 0;
                case DeviceConditionOperator.Less:
                    return compareValue > 0;
                case DeviceConditionOperator.LessOrEqual:
                    return compareValue >= 0;
                default:
                    return compareValue == 0;
            }
        }
    }
}
