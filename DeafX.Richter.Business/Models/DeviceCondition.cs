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

    public class DeviceCondition : ITriggerCondition
    {
        private IComparable _compareValue;
        private DeviceConditionOperator _compareOperator;
        private IDevice _device;

        public bool Fullfilled { get; private set; }

        public event OnTriggerConditionFullfilledHandler OnConditionFullfilled;

        public DeviceCondition(IDevice device, IComparable compareValue, DeviceConditionOperator compareOperator)
        {
            _device = device;
            _compareOperator = compareOperator;
            _compareValue = compareValue;

            device.OnValueChanged += (sender) => TestDeviceValue();

            TestDeviceValue();
        }

        public void Reset()
        {
            Fullfilled = false;
            TestDeviceValue();
        }

        private void TestDeviceValue()
        {
            if(!Fullfilled && CompareWithOperator(_compareValue.CompareTo(_device.Value)))
            {
                Fullfilled = true;
                OnConditionFullfilled?.Invoke(this);
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
