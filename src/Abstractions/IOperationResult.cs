using System.Collections.Generic;
using System.Text.Json.Nodes;
using Zs.Common.Enums;

namespace Zs.Common.Abstractions;

/// <summary> Any operation result </summary>
public interface IOperationResult
{
    bool IsSuccess { get; }
    bool HasWarnings { get; }
    IList<IInfoMessage> Messages { get; }
    void AddMessage(IInfoMessage message);
    void Merge(IOperationResult otherOperationResult);

    string JoinMessages(params InfoMessageType[] messageTypes);
    JsonObject ToJSON(params InfoMessageType[] messageTypes);
}

/// <summary> Any operation result </summary>
public interface IOperationResult<TResult> : IOperationResult
{
    TResult Value { get; }
}
