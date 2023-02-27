﻿using System;
using System.Linq;

namespace Zs.Common.Extensions;

public static class TimeSpanExtensions
{
    public static string ToDayHHmmss(this TimeSpan timeSpan)
    {
        // TODO: handle different languages
        var days = "";
        if (timeSpan.TotalDays >= 1)
        {
            days = $"{Math.Floor(timeSpan.TotalDays):0}";
            var lastDayDigit = (int)char.GetNumericValue(days.Last());
            var lastTwoDayDigits = (int)(timeSpan.TotalDays % 100);

            days += lastDayDigit switch
            {
                1 when lastTwoDayDigits != 11 => " день ",
                > 1 and < 5 when lastTwoDayDigits is < 11 or > 14 => " дня ",
                _ => " дней "
            };
        }

        return $"{days}{timeSpan.Hours:00}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
    }
}