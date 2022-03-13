using System;

namespace Zs.Common.Extensions
{
    public static class ExceptionExtension
    {
        public static string ToText(this Exception ex)
            => $"\n\n{ex}\nMessage:\n{ex.Message}\n\nStackTrace:\n{ex.StackTrace}";
    }
}
