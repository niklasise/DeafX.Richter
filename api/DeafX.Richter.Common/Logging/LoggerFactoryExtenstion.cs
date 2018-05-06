using DeafX.Richter.Common.DataStorage;
using DeafX.Richter.Common.Logging;

namespace Microsoft.Extensions.Logging
{
    public static class LoggerFactoryExtenstion
    {
        public static void AddDatabase(this ILoggingBuilder loggingBuilder, IDataOverTimeStorage dataStorage, LogLevel logLevel = LogLevel.Warning)
        {
            loggingBuilder.AddProvider(new DatabaseLoggerProvider(dataStorage, logLevel));
        }
    }
}
