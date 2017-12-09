using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeafX.Richter.Common.DataStorage
{
    public class LiteDbDataStorage : IDataOverTimeStorage
    {
        private string _storagePath;

        public LiteDbDataStorage(string storagePath)
        {
            _storagePath = storagePath;
        }

        public IEnumerable<T> RetreiveRecent<T>(int count)
        {
            using (var db = new LiteDatabase(_storagePath))
            {
                var collection = db.GetCollection<DataTimeObject<T>>(nameof(T));

                return collection.Find(Query.All(Query.Descending), limit: 100).Select(o => o.Data).ToArray();
            }
        }

        public IEnumerable<T> Retreive<T>(DateTime from)
        {
            return Retreive<T>(from, DateTime.MaxValue);
        }

        public IEnumerable<T> Retreive<T>(DateTime from, DateTime to)
        {
            using (var db = new LiteDatabase(_storagePath))
            {
                var collection = db.GetCollection<DataTimeObject<T>>(nameof(T));

                return collection.Find(o => o.DateTime >= from && o.DateTime <= to).Select(o => o.Data).ToArray();
            }
        }

        public void Store<T>(T data)
        {
            StoreData(data, DateTime.Now);
        }

        public void Store<T>(T data, Func<T, DateTime> dateTimeSelector)
        {
            StoreData(data, dateTimeSelector(data));
        }

        public void StoreData<T>(T data, DateTime dateTime)
        {
            using (var db = new LiteDatabase(_storagePath))
            {
                var collection = db.GetCollection<DataTimeObject<T>>(nameof(T));

                collection.Insert(new DataTimeObject<T>()
                {
                    Id = ObjectId.NewObjectId(),
                    Data = data,
                    DateTime = dateTime
                });

                collection.EnsureIndex(o => o.DateTime);
            }
        }

        private class DataTimeObject<T>
        {
            public ObjectId Id { get; set; }

            public DateTime DateTime { get; set; }

            public T Data { get; set; }
        }

    }
}
