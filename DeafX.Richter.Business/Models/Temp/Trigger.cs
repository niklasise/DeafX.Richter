//using DeafX.Richter.Business.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace DeafX.Richter.Business.Models
//{
//    public class Trigger<T> : ValueObject<bool> where T : IComparable
//    {
//        private List<ITriggerCondition> _conditions;
//        private IDevice<T> _device;
//        private T _value;

//        public Trigger(ITriggerCondition condition, IDevice<T> device, T value)
//            : this(new List<ITriggerCondition>() { condition }, device, value) { }

//        public Trigger(List<ITriggerCondition> conditions, IDevice<T> device, T value)
//        {
//            _conditions = conditions;
//            _device = device;
//            _value = value;
            
//            _conditions.ForEach(c => c.ValueChanged += OnTriggerConditionChanged);
//        }

//        private void OnTriggerConditionChanged(object sender, ValueObjectChangedEventArgs<bool> args)
//        {
//            if(_conditions.All(c => c.Value))
//            {
//                _device.Value = _value;
//            }
//        }
//    }
//}
