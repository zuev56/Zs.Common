using System;
using System.IO;
using Zs.Common.Enums;

namespace Zs.Common.Models;

public static class ProgramUtilites
{
    /// <summary>
    /// Returns full path to appsettings.json
    /// </summary>
    [Obsolete("Use GetAppsettingsPath instead")]
    public static string MainConfigurationPath
        => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");

    /// <summary>
    /// Returns full path to appsettings.json or appsettings.Development.json, depends on Environment 
    /// </summary>
    public static string GetAppsettingsPath(AppEnvironment environment = default)
    {
        var fileName = environment is AppEnvironment.Default
            ? "appsettings.json"
            : $"appsettings.{environment}.json";

        return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
    }

    public static void TrySaveFailInfo(string text)
    {
        try
        {
            string path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), $"Critical_failure_{DateTime.Now:yyyy.MM.dd HH:mm:ss.ff}.log");
            File.AppendAllText(path, text);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n\n{ex}\nMessage:\n{ex.Message}\n\nStackTrace:\n{ex.StackTrace}");
        }
    }

}
