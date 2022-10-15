using System;
using Zs.Common.Abstractions;
using Zs.Common.Enums;

namespace Zs.Common.Models;

[Obsolete("Will be removed in version 7.x.x")]
public sealed class InfoMessage : IInfoMessage
{
    public InfoMessageType Type { get; init; }
    public string Text { get; init; }


    public static InfoMessage Success(string text)
        => new InfoMessage
        {
            Type = InfoMessageType.Info,
            Text = text
        };

    public static InfoMessage Warning(string text)
        => new InfoMessage
        {
            Type = InfoMessageType.Warning,
            Text = text
        };

    public static InfoMessage Error(string text)
        => new InfoMessage
        {
            Type = InfoMessageType.Error,
            Text = text
        };
}