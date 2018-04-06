using System;
using System.Collections.Generic;
using System.Text;

namespace DeafX.Richter.Common.DataStorage
{
    public interface IDataOverTimeStorage
    {

        void Store<T>(string name, T data);

        void Store<T>(string name, T data, Func<T, DateTime> dateTimeSelector);

        void Store<T>(string name, T data, DateTime dateTime);

        void StoreAll<T>(string name, Dictionary<DateTime, T> data);

        IEnumerable<DataTimeObject<T>> RetreiveRecent<T>(string name, int count);

        IEnumerable<DataTimeObject<T>> Retreive<T>(string name, DateTime from);

        IEnumerable<DataTimeObject<T>> Retreive<T>(string name, DateTime from, DateTime to);

    }

    public class DataTimeObject<T> : IComparable<DataTimeObject<T>>
    {
        public DateTime DateTime { get; set; }

        public T Data { get; set; }
        
        public int CompareTo(DataTimeObject<T> other)
        {
            return DateTime.CompareTo(other.DateTime);
        }
    }
}
