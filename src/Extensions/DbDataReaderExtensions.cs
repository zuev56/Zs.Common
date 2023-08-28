using System.Collections.Generic;
using System.Data.Common;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace Zs.Common.Extensions;

public static class DbDataReaderExtensions
{
    private static readonly JsonSerializerOptions PrettyJsonSerializerOptions = new ()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public static async Task<string> ReadToJsonAsync(this DbDataReader reader)
    {
        var rows = new List<Dictionary<string, string?>>();
        while (await reader.ReadAsync().ConfigureAwait(false))
        {
            var row = new Dictionary<string, string?>();
            var columnSchema = await reader.GetColumnSchemaAsync().ConfigureAwait(false);

            for (var i = 0; i < reader.FieldCount; i++)
            {
                var baseColumnName = columnSchema[i].BaseColumnName;
                if (baseColumnName != null)
                {
                    row.Add(baseColumnName, reader[i].ToString());
                }
            }

            rows.Add(row);
        }

        return JsonSerializer.Serialize(rows, PrettyJsonSerializerOptions);
    }
}