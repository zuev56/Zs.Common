using System;

namespace Zs.Common.Models
{
    public sealed class ErrorInfo
    {
        public string Text { get; }
        public DateTime DateTime { get; }

        public ErrorInfo(string text)
        {
            Text = text;
            DateTime = DateTime.UtcNow;
        }

        public ErrorInfo(string text, DateTime dateTime)
        {
            Text = text;
            DateTime = dateTime;
        }
    }
}
