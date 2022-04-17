using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using Zs.Common.Abstractions;
using Zs.Common.Enums;

namespace Zs.Common.Models;

public class ServiceResult : IOperationResult
{
    // TODO: remove  m != null and fix
    public bool IsSuccess => Messages.All(m => m.Type != InfoMessageType.Error);
    public bool HasWarnings => Messages.Any(m => m.Type == InfoMessageType.Warning);
    public IList<IInfoMessage> Messages { get; } = new List<IInfoMessage>(1);

    protected ServiceResult()
    {
    }

    public static ServiceResult Success(string message = null)
    {
        var result = new ServiceResult();

        if (!string.IsNullOrWhiteSpace(message))
            result.Messages.Add(InfoMessage.Success(message));

        return result;
    }

    public static ServiceResult Warning(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message can not be empty", nameof(message));

        var result = new ServiceResult();

        if (!string.IsNullOrWhiteSpace(message))
            result.Messages.Add(InfoMessage.Warning(message));

        return result;
    }

    public static ServiceResult Error(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message can not be empty", nameof(message));

        var result = new ServiceResult();

        if (!string.IsNullOrWhiteSpace(message))
            result.Messages.Add(InfoMessage.Error(message));

        return result;
    }

    public static ServiceResult ErrorFrom(IOperationResult other)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other));

        var serviceResult = new ServiceResult();
        serviceResult.Merge(other);

        return serviceResult;
    }


    public void AddMessage(IInfoMessage message)
    {
        if (message == null)
            throw new ArgumentNullException(nameof(message));

        Messages.Add(message);
    }

    public void AddMessage(string message, InfoMessageType type = InfoMessageType.Info)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message can not be empty", nameof(message));

        var infoMessage = new InfoMessage
        {
            Type = type,
            Text = message
        };
        AddMessage(infoMessage);
    }

    public void Merge(IOperationResult otherOperationResult)
    {
        if (otherOperationResult == null)
            throw new ArgumentNullException(nameof(otherOperationResult));

        ((List<IInfoMessage>)Messages).AddRange(otherOperationResult.Messages);
    }

    public virtual string JoinMessages(params InfoMessageType[] messageTypes)
    {
        var messages = messageTypes?.Length > 0
            ? Messages.Where(m => messageTypes.Contains(m.Type)).ToList()
            : Messages;

        return string.Join(Environment.NewLine, messages.Select(m => $"[{m.Type}]: {m.Text}"));
    }

    public virtual JsonObject ToJSON(params InfoMessageType[] messageTypes)
    {
        var messages = Messages.Select(m => new JsonObject { [m.Type.ToString()] = m.Text }).ToArray();

        return new JsonObject
        {
            [nameof(IsSuccess)] = IsSuccess,
            [nameof(Messages)] = new JsonArray(messages)
        };
    }
}

public class ServiceResult<TResult> : ServiceResult, IOperationResult<TResult>
{
    public TResult Value { get; init; }

    protected ServiceResult()
    {
    }

    public static ServiceResult<TResult> Success(TResult result, string message = null)
    {
        var serviceResult = new ServiceResult<TResult> { Value = result };

        if (!string.IsNullOrWhiteSpace(message))
            serviceResult.Messages.Add(InfoMessage.Success(message));

        return serviceResult;
    }

    [Obsolete]
    public static ServiceResult<TResult> Warning(TResult result, string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message can not be empty", nameof(message));

        var serviceResult = new ServiceResult<TResult> { Value = result };
        serviceResult.Messages.Add(InfoMessage.Warning(message));

        return serviceResult;
    }

    public static ServiceResult<TResult> Warning(string message, TResult result = default)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message can not be empty", nameof(message));

        var serviceResult = new ServiceResult<TResult> { Value = result };
        serviceResult.Messages.Add(InfoMessage.Warning(message));

        return serviceResult;
    }

    public static ServiceResult<TResult> Error(string message, TResult result = default)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message can not be empty", nameof(message));

        var serviceResult = new ServiceResult<TResult> { Value = result };
        serviceResult.Messages.Add(InfoMessage.Error(message));

        return serviceResult;
    }

    public new static ServiceResult<TResult> ErrorFrom(IOperationResult other)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other));

        var serviceResult = new ServiceResult<TResult>();
        serviceResult.Merge(other);

        return serviceResult;
    }

    public override JsonObject ToJSON(params InfoMessageType[] messageTypes)
    {
        JsonObject jsonObject = base.ToJSON(messageTypes);
        jsonObject.Add(nameof(Value), JsonNode.Parse(JsonSerializer.Serialize(Value)));

        return jsonObject;
    }
}
