using System;
using Zs.Common.Abstractions;

namespace Zs.Common.Extensions;

public static class OperationResultExtension
{
    public static void AssertResultIsSuccessful(this IOperationResult result)
    {
        if (!result.IsSuccess)
        {
            var exception = new InvalidOperationException(result.JoinMessages());
            exception.Data.Add("Result", result);
            throw exception;
        }
    }
}
