using System;

namespace TestApp
{
    public static class TimeSpanExtensions
    {
        public static bool IsBetween(this TimeSpan timeSpan, TimeSpan from, TimeSpan to)
        {
            return timeSpan >= from && timeSpan <= to;
        }
    }
}
