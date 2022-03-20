using System;
using System.IO;

namespace Zs.Common.Models;

public static class ProgramUtilites
{
    public static string MainConfigurationPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");

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
