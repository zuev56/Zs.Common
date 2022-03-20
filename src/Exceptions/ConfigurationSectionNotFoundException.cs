using System;

namespace Zs.Common.Exceptions;

public sealed class ConfigurationSectionNotFoundException : Exception
{
    public ConfigurationSectionNotFoundException(string section)
        : base($"Section '{section}' is not found in configuration")
    {
    }
}
