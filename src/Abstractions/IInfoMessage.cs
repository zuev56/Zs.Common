using Zs.Common.Enums;

namespace Zs.Common.Abstractions
{
    public interface IInfoMessage
    {
        InfoMessageType Type { get; }
        string Text { get; }
    }
}
