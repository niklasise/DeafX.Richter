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

        public IEnumerable<DataTimeObject<T>> RetreiveRecent<T>(string name, int count)
        {
            using (var db = new LiteDatabase(_storagePath))
            {
                if (!db.CollectionExists(name))
                {
                    return null;
                }

                var collection = db.GetCollection<LiteDbDataTimeObject<T>>(nameof(T));

                return collection.Find(Query.All(Query.Descending), limit: 100).ToArray();
            }
        }

        public IEnumerable<DataTimeObject<T>> Retreive<T>(string name, DateTime from)
        {
            return Retreive<T>(name, from, DateTime.MaxValue);
        }

        public IEnumerable<DataTimeObject<T>> Retreive<T>(string name, DateTime from, DateTime to)
        {
            using (var db = new LiteDatabase(_storagePath))
            {
                if (!db.CollectionExists(name))
                {
                    return null;
                }

                var collection = db.GetCollection<LiteDbDataTimeObject<T>>(name);

                return collection.Find(o => o.DateTime >= from && o.DateTime <= to).ToArray();
            }
        }

        public void Store<T>(string name, T data)
        {
            StoreData(name, data, DateTime.Now);
        }

        public void Store<T>(string name, T data,  Func<T, DateTime> dateTimeSelector)
        {
            StoreData(name, data, dateTimeSelector(data));
        }

        public void StoreData<T>(string name, T data, DateTime dateTime)
        {
            using (var db = new LiteDatabase(_storagePath))
            {
                var collection = db.GetCollection<LiteDbDataTimeObject<T>>(name);

                collection.Insert(new LiteDbDataTimeObject<T>()
                {
                    Id = ObjectId.NewObjectId(),
                    Data = data,
                    DateTime = dateTime
                });

                collection.EnsureIndex(o => o.DateTime);
            }
        }

        private class LiteDbDataTimeObject<T> : DataTimeObject<T>
        {
            public ObjectId Id { get; set; }
        }

    }
}
