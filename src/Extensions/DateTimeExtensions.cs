using System;

namespace Zs.Common.Extensions;

public static class DateTimeExtensions
{
    private static readonly DateTime UnixEpoch = new (1970,1,1,0,0,0, DateTimeKind.Utc);

    public static DateTime NextHour(this DateTime _)
    {
        var nextHour = DateTime.UtcNow.Hour < 23
            ? DateTime.UtcNow.Date + TimeSpan.FromHours(1)
            : DateTime.UtcNow.Date + TimeSpan.FromDays(1);

        while (DateTime.UtcNow > nextHour)
        {
            nextHour += TimeSpan.FromHours(1);
        }

        return nextHour;
    }

    public static DateTime FromUnixEpoch(this int seconds) => UnixEpoch + TimeSpan.FromSeconds(seconds);

    public static int ToUnixEpoch(this DateTime dateTime) => (int)dateTime.Subtract(UnixEpoch).TotalSeconds;
}