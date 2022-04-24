using System;

namespace Zs.Common.Extensions;

public static class LongExtension
{
    public static string BytesAmountToSizeString(this long bytesAmount)
    {
        var units = new[] { "Bytes", "KB", "MB", "GB", "TB" };

        if (bytesAmount == 0L)
        {
            return "0 Byte";
        }

        int num = (int)Math.Floor(Math.Log(bytesAmount) / Math.Log(1024.0));
        return Math.Round((double)bytesAmount / Math.Pow(1024.0, num), 2) + " " + units[num];
    }


}
