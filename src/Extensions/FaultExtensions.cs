using System;
using Zs.Common.Models;

namespace Zs.Common.Extensions;

public static class FaultExtensions
{
    [Obsolete("Use WithMessage instead")]
    public static Fault SetMessage(this Fault fault, string message)
    {
        return fault.WithMessage(message);
    }

    public static Fault WithMessage(this Fault fault, string message)
    {
        ArgumentNullException.ThrowIfNull(message);
        return new Fault(fault.Code, message, fault.Details);
    }
}