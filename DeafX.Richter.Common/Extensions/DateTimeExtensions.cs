using System;
using System.Collections.Generic;
using System.Text;

namespace DeafX.Richter.Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static long ToUnixTimeStamp(this DateTime dateTime)
        {
            var dateTimeOffset = new DateTimeOffset(dateTime);
            return dateTimeOffset.ToUnixTimeSeconds();
        }

        public static DateTime FromUnixTimeStamp(long timeStamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(timeStamp).DateTime.ToLocalTime();
        }
    }
}
