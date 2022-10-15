using System;

namespace Zs.Common.Models;

[Obsolete("Will be removed in version 7.x.x")]
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