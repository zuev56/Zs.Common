using System;
using Zs.Common.Enums;

namespace Zs.Common.Abstractions;

[Obsolete("Will be removed in version 7.x.x")]
public interface IInfoMessage
{
    InfoMessageType Type { get; }
    string Text { get; }
}