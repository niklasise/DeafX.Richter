using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeafX.Richter.Common.Logging
{
    public class LogItem
    {
        public DateTime DateTime { get; set; }

        public LogLevel LogLevel { get; set; }

        public EventId EventId { get; set; }

        public string Exception { get; set; }

        public string Message { get; set; }

        public string Category { get; set; }

    }
}
