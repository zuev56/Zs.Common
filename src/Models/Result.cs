using System;
using Zs.Common.Exceptions;

namespace Zs.Common.Models;

public class Result
{
    public Fault? Fault { get; }
    public bool Successful => Fault == null;

    protected Result(Fault? fault)
    {
        Fault = fault;
    }

    public static Result Success() => new(null);

    public static Result Fail(Fault fault)
    {
        ArgumentNullException.ThrowIfNull(fault);
        return new Result(fault);
    }

    public virtual void EnsureSuccess()
    {
        if (!Successful)
        {
            throw new FaultException(Fault!);
        }
    }
}

public class Result<TValue> : Result
{
    private readonly TValue _value;

    public TValue Value
    {
        get
        {
            EnsureSuccess();
            return _value;
        }
    }

    public Result(TValue value, Fault? fault = null)
        : base(fault)
    {
        _value = value;
    }

    public static Result<TValue> Success(TValue value) => new(value);
    public static Result<TValue> Fail<TValue>(Fault fault)
    {
        ArgumentNullException.ThrowIfNull(fault);
        return new Result<TValue>(default!, fault);
    }
}