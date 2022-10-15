using System;
using Zs.Common.Extensions;
using Zs.Common.Models;

namespace Zs.Common.Exceptions;

public class FaultException : Exception
{
    public Fault Fault { get; }

    public FaultException(Fault fault, Exception? innerException)
        : base(fault.Message, innerException)
    {
        Fault = fault;
    }

    public FaultException(Fault fault)
        : this(fault, null)
    {
    }

    public FaultException()
        : this(Fault.Unknown)
    {
    }

    public FaultException(string message, Exception innerException)
        : this(Fault.Unknown.SetMessage(message), innerException)
    {
    }

    public FaultException(string message)
        : this(message, null)
    {
    }
}