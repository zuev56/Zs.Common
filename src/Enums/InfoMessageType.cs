using System;

namespace Zs.Common.Enums;

[Obsolete("Will be removed in version 7.x.x")]
public enum InfoMessageType : short
{
    Info = 0,
    Warning,
    Error
}