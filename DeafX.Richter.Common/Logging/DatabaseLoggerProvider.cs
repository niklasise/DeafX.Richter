using DeafX.Richter.Common.DataStorage;
using Microsoft.Extensions.Logging;

namespace DeafX.Richter.Common.Logging
{
    public class DatabaseLoggerProvider : ILoggerProvider
    {
        public LogLevel LogLevel { get; set; }

        private IDataOverTimeStorage _dataStorage;

        public DatabaseLoggerProvider(IDataOverTimeStorage dataStorage, LogLevel logLevel)
        {
            _dataStorage = dataStorage;
            LogLevel = logLevel;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new DatabaseLogger(categoryName, () => LogLevel, LogItem);
        }

        public void Dispose()
        {
            
        }

        private void LogItem(LogItem logItem)
        {
            _dataStorage.Store(logItem, i => i.DateTime);
        }
    }
}
