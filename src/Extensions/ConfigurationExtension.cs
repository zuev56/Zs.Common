using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Zs.Common.Extensions
{
    public static class ConfigurationExtension
    {
        /// <summary>
        /// Gets a configuration value.
        /// If the value has {interpolation expression}, the method tries to replace it 
        /// with matching value in [Secrets] section. Otherwise it returns raw value
        /// </summary>
        public static string GetSecretValue(this IConfiguration configuration, string key)
        {
            if (key is null)
                throw new ArgumentNullException(nameof(key));

            var value = configuration[key];
            if (value != null && Regex.IsMatch(value, @"\{(.*?)\}"))
            {
                var nodeWithValueToReplace = configuration.GetSection($"Secrets:{Regex.Match(value, @"(?<=\{)[^}]*(?=\})")?.Value}");

                value = nodeWithValueToReplace?.Value != null
                    ? Regex.Replace(value, @"\{(.*?)\}", nodeWithValueToReplace.Value)
                    : value;
            }

            return value;
        }

        public static bool ContainsKey(this IConfiguration configuration, string key)
        {
            if (key is null)
                throw new ArgumentNullException(nameof(key));

            return configuration.GetChildren().Any(item => item.Key == key);
        }
        
    }
}
