using System;

namespace DeafX.Richter.Business.Exceptions
{
    public class ZWayException : Exception
    {
        public ZWayException() { }

        public ZWayException(string message) : base(message) { }

        public ZWayException(string message, Exception innerException) : base(message, innerException) { }
    }
}
