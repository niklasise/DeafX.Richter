//using DeafX.Richter.Business.Interfaces;
//using System;

//namespace DeafX.Richter.Business.Models
//{
//    public enum TriggerConditionOperator
//    {
//        Equal,
//        Less,
//        LessOrEqual,
//        Greater,
//        GreaterOrEqual
//    }

//    public class TriggerCondition<T> : ValueObject<bool>, ITriggerCondition where T : IComparable
//    {
//        private IDevice<T> _triggerDevice;
//        private T _compareValue;
//        private TriggerConditionOperator _oprtr;

//        public TriggerCondition(IDevice<T> triggerDevice, T compareValue, TriggerConditionOperator oprtr)
//        {
//            if(compareValue == null)
//            {
//                throw new ArgumentNullException(nameof(compareValue));
//            }

//            _triggerDevice = triggerDevice;
//            _compareValue = compareValue;
//            _oprtr = oprtr;

//            _triggerDevice.ValueChanged += OnTriggerValueChanged;
//        }

//        ~TriggerCondition()
//        {
//            _triggerDevice.ValueChanged -= OnTriggerValueChanged;
//        }

//        private void OnTriggerValueChanged(object sender, ValueObjectChangedEventArgs<T> args)
//        {
//            Value = GetTriggerValue(_compareValue.CompareTo(args.NewValue));
//        }

//        private bool GetTriggerValue(int compareValue)
//        {
//            switch (_oprtr)
//            {
//                case TriggerConditionOperator.Greater:
//                    return compareValue > 0;
//                case TriggerConditionOperator.GreaterOrEqual:
//                    return compareValue >= 0;
//                case TriggerConditionOperator.Less:
//                    return compareValue < 0;
//                case TriggerConditionOperator.LessOrEqual:
//                    return compareValue <= 0;
//                default:
//                    return compareValue == 0;
//            }
//        }

//    }
//}
