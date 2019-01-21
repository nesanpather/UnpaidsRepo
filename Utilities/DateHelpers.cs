using System;

namespace Utilities
{
    public static class DateHelpers
    {
        public static DateTime StartOfDay(this DateTime theDate)
        {
            return theDate.Date.ToUniversalTime();
        }

        public static DateTime EndOfDay(this DateTime theDate)
        {
            return theDate.Date.ToUniversalTime().AddDays(1).AddTicks(-1);
        }
    }
}
