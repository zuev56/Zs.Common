using System;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Zs.Common.Enums;
using Zs.Common.Extensions;

namespace Zs.Common.Abstractions;

public abstract class DbClientBase<TConnection, TCommand> : IDbClient
    where TConnection : DbConnection, new()
    where TCommand : DbCommand, new()
{
    private readonly string _connectionString;
    private readonly ILogger<DbClientBase<TConnection, TCommand>>? _logger;

    protected DbClientBase(string connectionString, ILogger<DbClientBase<TConnection, TCommand>>? logger = null)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException($"'{nameof(_connectionString)}' cannot be null or empty", nameof(_connectionString));
        }

        _connectionString = connectionString;
        _logger = logger;
    }

    public async Task<string?> GetQueryResultAsync(string sqlQuery)
    {
        var result = await MakeQueryAndHandleResult(sqlQuery,
            handleResult: static async (reader) =>
            {
                await reader.ReadAsync().ConfigureAwait(false);;

                var isDbNull = await reader.IsDBNullAsync(0).ConfigureAwait(false);;
                var result = !isDbNull ? reader.GetString(0) : null;

                return result;
            }).ConfigureAwait(false);;

        return result;
    }

    public async Task<string?> GetQueryResultAsync(string sqlQuery, QueryResultType resultType)
    {
        return await MakeQueryAndHandleResult(sqlQuery,
            handleResult: async reader =>
            {
                bool isDbNull;
                switch (resultType)
                {
                    case QueryResultType.Double:
                        await reader.ReadAsync().ConfigureAwait(false);;
                        isDbNull = await reader.IsDBNullAsync(0).ConfigureAwait(false);;
                        return isDbNull ? null : reader.GetDouble(0).ToString(CultureInfo.InvariantCulture);
                    case QueryResultType.Json:
                        return await reader.ReadToJsonAsync().ConfigureAwait(false);;
                    case QueryResultType.String:
                        await reader.ReadAsync().ConfigureAwait(false);;
                        isDbNull = await reader.IsDBNullAsync(0).ConfigureAwait(false);;
                        return isDbNull ? null : reader.GetString(0);
                    default:
                        return null;
                }
            }).ConfigureAwait(false);;
    }

    private async Task<TResult> MakeQueryAndHandleResult<TResult>(string sqlQuery, Func<DbDataReader, Task<TResult>> handleResult)
    {
        try
        {
            if (string.IsNullOrEmpty(sqlQuery))
                throw new ArgumentException($"'{nameof(sqlQuery)}' cannot be null or empty", nameof(sqlQuery));

            var sw = new Stopwatch();
            sw.Start();

            await using var connection = new TConnection();
            {
                connection.ConnectionString = _connectionString;
                await connection.OpenAsync().ConfigureAwait(false);;
                TResult result;

                await using var command = new TCommand();
                {
                    command.CommandText = sqlQuery;
                    command.Connection = connection;

                    await using var reader = await command.ExecuteReaderAsync().ConfigureAwait(false);;
                    {
                        result = await handleResult(reader);
                    }
                }

                sw.Stop();
                _logger?.LogDebug("{MethodName} [Elapsed: {Elapsed}].\n\tSQL: {SQL}", nameof(GetQueryResultAsync), sw.Elapsed, sqlQuery);

                await connection.CloseAsync().ConfigureAwait(false);;
                return result;
            }
        }
        catch (InvalidCastException icex)
        {
            _logger?.LogError(icex, "Error getting query result");
            throw;
        }
    }
}