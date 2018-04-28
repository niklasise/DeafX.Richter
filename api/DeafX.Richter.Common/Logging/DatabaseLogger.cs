using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeafX.Richter.Common.Logging
{
    public class DatabaseLogger : ILogger, IDisposable
    {
        private Func<LogLevel> _logLevel;
        private Action<LogItem> _logItem;
        private string _category;

        public DatabaseLogger(string category, Func<LogLevel> logLevel, Action<LogItem> logItem)
        {
            _logLevel = logLevel;
            _logItem = logItem;
            _category = category;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return this;   
        }

        public void Dispose()
        {
            
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= _logLevel();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if(!IsEnabled(logLevel))
            {
                return;
            }

            _logItem(
                new LogItem()
                {
                    DateTime = DateTime.Now,
                    EventId = eventId,
                    LogLevel = logLevel,
                    Exception = exception != null ? $"{exception.GetType().Name} - {exception.Message}"  : null,
                    Message = formatter != null ? formatter(state, exception) : null,
                    Category = _category
                }
            );
        }
    }
}
