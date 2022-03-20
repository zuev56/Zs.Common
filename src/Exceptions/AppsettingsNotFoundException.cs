using System;
using System.IO;

namespace Zs.Common.Exceptions;

public sealed class AppsettingsNotFoundException : FileNotFoundException
{
    public AppsettingsNotFoundException()
        : base($"Configuration file appsettings.json is not found in the application directory: {AppDomain.CurrentDomain.BaseDirectory}")
    {
    }
}
