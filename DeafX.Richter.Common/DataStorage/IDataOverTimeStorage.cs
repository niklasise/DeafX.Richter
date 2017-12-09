using System;
using System.Collections.Generic;
using System.Text;

namespace DeafX.Richter.Common.DataStorage
{
    public interface IDataOverTimeStorage
    {

        void Store<T>(T data);

        void Store<T>(T data, Func<T, DateTime> dateTimeSelector);

        IEnumerable<T> RetreiveRecent<T>(int count);

        IEnumerable<T> Retreive<T>(DateTime from);

        IEnumerable<T> Retreive<T>(DateTime from, DateTime to);

    }
}
