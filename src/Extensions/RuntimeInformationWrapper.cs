using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Zs.Common.Extensions
{
    public static class RuntimeInformationWrapper
    {
        public static string GetRuntimeInfo()
        {
            var sb = new StringBuilder()
              .Append("OS: ").Append(RuntimeInformation.OSDescription).Append(' ').Append(RuntimeInformation.OSArchitecture).AppendLine()
              .Append("Framework: ").Append(RuntimeInformation.FrameworkDescription).AppendLine()
              .Append("Process: ").Append(RuntimeInformation.ProcessArchitecture).AppendLine()
              .Append("RintimeID: ").Append(RuntimeInformation.RuntimeIdentifier);

            return sb.ToString();
        }
    }
}
