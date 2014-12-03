using System;

namespace AlgorithmicTrading.Common.Utilities
{
    public static class DateTimeExtensions
    {
        private static readonly DateTime Jan1St1970 = new DateTime(1970, 1, 1, 0, 0, 0);

        public static long ToMilliseconds(this DateTime date)
        {
            return (long)((date - Jan1St1970).TotalMilliseconds);
        }

        public static DateTime ToTime(this long milliseconds)
        {
            return Jan1St1970.AddMilliseconds(milliseconds);
        }

        public static string GetDisplayString(this DateTime time)
        {
            if (time == DateTime.MinValue)
            {
                return string.Empty;
            }

            if (time.TimeOfDay == TimeSpan.Zero)
            {
                return time.ToString("yy-MM-dd");
            }

            return time.ToString("yy-MM-dd hh:mm:ss");
        }
    }
}