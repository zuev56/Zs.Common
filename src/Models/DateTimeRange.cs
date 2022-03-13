using System;

namespace Zs.Common.Models
{
    public sealed class DateTimeRange
    {
        public DateTime Start { get; init; }
        public DateTime End { get; init; }

        private DateTimeRange()
        {
        }

        public DateTimeRange(DateTime start)
        {
            Start = start;
        }

        public DateTimeRange(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }
    }
}
