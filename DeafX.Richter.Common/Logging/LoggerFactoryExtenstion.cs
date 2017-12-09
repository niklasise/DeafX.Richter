using DeafX.Richter.Common.DataStorage;
using DeafX.Richter.Common.Logging;

namespace Microsoft.Extensions.Logging
{
    public static class LoggerFactoryExtenstion
    {
        public static void AddDatabase(this ILoggerFactory loggerFactory, IDataOverTimeStorage dataStorage)
        {
            loggerFactory.AddProvider(new DatabaseLoggerProvider(dataStorage));
        }
    }
}
