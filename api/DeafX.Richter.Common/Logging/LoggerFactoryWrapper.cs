using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeafX.Richter.Common.Logging
{
    public static class LoggerFactoryWrapper
    {
        public static ILoggerFactory LoggerFactory { get; set; }

        public static ILogger<T> CreateLogger<T>()
        {
            if (LoggerFactory == null)
            {
                return new EmptyLogger<T>();
            }

            return LoggerFactory.CreateLogger<T>();
        }
    }
}
