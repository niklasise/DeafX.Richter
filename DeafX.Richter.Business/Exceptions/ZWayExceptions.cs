using System;

namespace DeafX.Richter.Business.Exceptions
{
    public class ZWayException : Exception
    {
        public ZWayException() { }

        public ZWayException(string message) : base(message) { }

        public ZWayException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class ZWayDeviceException : Exception
    {
        public ZWayDeviceException() { }

        public ZWayDeviceException(string message) : base(message) { }

        public ZWayDeviceException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class ZWayDeviceConfigurationException : Exception
    {
        public ZWayDeviceConfigurationException() { }

        public ZWayDeviceConfigurationException(string message) : base(message) { }

        public ZWayDeviceConfigurationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
