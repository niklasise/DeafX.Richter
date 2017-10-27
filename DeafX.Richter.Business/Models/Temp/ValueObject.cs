//using System;

//namespace DeafX.Richter.Business.Models
//{
//    public delegate void ValueObjectChangedHandler<T>(object sender, ValueObjectChangedEventArgs<T> args);

//    public class ValueObjectChangedEventArgs<T> : EventArgs
//    {
//        public ValueObjectChangedEventArgs(T previousValue, T newValue)
//        {
//            PreviousValue = previousValue;
//            NewValue = newValue;
//        }

//        public T PreviousValue { get; private set; }
//        public T NewValue { get; private set; }
//    }

//    public abstract class ValueObject<T> where T : IComparable
//    {
//        public DateTime LastChanged
//        {
//            get; private set;
//        }

//        public T Value
//        {
//            get
//            {
//                return _value;
//            }
//            set
//            {
//                SetValue(value);
//            }
//        }

//        public event ValueObjectChangedHandler<T> ValueChanged;

//        private T _value;

//        private void SetValue(T value)
//        {
//            // Return if value is same as before
//            if ((value == null && _value == null) ||
//               (value != null && value.CompareTo(_value) == 0))
//            {
//                return;
//            }

//            var eventArgs = new ValueObjectChangedEventArgs<T>(
//                previousValue: _value,
//                newValue: value
//            );

//            LastChanged = DateTime.Now;
//            _value = value;

//            ValueChanged?.Invoke(this, eventArgs);
//        }

//    }
//}
