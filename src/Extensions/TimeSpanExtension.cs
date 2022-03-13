using System;
using System.Linq;

namespace Zs.Common.Extensions
{
    public static class TimeSpanExtentions
    {
        public static string ToDayHHmmss(this TimeSpan timeSpan)
        {
            string days = "";
            if (timeSpan.TotalDays >= 1)
            {
                days = $"{Math.Floor(timeSpan.TotalDays):0}";
                int lastDayDigit = (int)char.GetNumericValue(days.Last());
                int lastTwoDayDigits = (int)(timeSpan.TotalDays % 100);


                if (lastDayDigit == 1 && lastTwoDayDigits != 11)
                    days += " день ";
                else if (lastDayDigit > 1 && lastDayDigit < 5 && (lastTwoDayDigits < 11 || lastTwoDayDigits > 14))
                    days += " дня ";
                else
                    days += " дней ";
            }

            return $"{days} {timeSpan.Hours:00}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
        }


    }
}
