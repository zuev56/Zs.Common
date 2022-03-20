using System.Collections.Generic;
using System.Data.Common;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace Zs.Common.Extensions;

public static class DbDataReaderExtension
{
    private static readonly JsonSerializerOptions prettyJsonSerializerOptions = new JsonSerializerOptions()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public async static Task<string> ReadToJsonAsync(this DbDataReader reader)
    {
        var rows = new List<Dictionary<string, string>>();

        while (await reader.ReadAsync().ConfigureAwait(false))
        {
            var row = new Dictionary<string, string>();
            var columnSchema = await reader.GetColumnSchemaAsync().ConfigureAwait(false);

            for (int i = 0; i < reader.FieldCount; i++)
                row.Add(columnSchema[i].BaseColumnName, reader[i].ToString());

            rows.Add(row);
        }

        return JsonSerializer.Serialize(rows, prettyJsonSerializerOptions);
    }
}
