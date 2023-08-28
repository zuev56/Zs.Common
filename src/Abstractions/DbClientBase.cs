using System;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
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
        ArgumentException.ThrowIfNullOrEmpty(connectionString);

        _connectionString = connectionString;
        _logger = logger;
    }

    public async Task<string?> GetQueryResultAsync(string sqlQuery, CancellationToken cancellationToken = default)
    {
        return await MakeQueryAndHandleResult(sqlQuery, HandleResult, cancellationToken).ConfigureAwait(false);

        async Task<string?> HandleResult(DbDataReader reader)
        {
            await reader.ReadAsync(cancellationToken).ConfigureAwait(false);
            var isDbNull = await reader.IsDBNullAsync(0, cancellationToken).ConfigureAwait(false);
            var result = isDbNull ? null : reader.GetString(0);

            return result;
        }
    }

    public async Task<string?> GetQueryResultAsync(string sqlQuery, QueryResultType resultType, CancellationToken cancellationToken = default)
    {
        return await MakeQueryAndHandleResult(sqlQuery, HandleResult, cancellationToken).ConfigureAwait(false);

        async Task<string?> HandleResult(DbDataReader reader)
        {
            bool isDbNull;
            switch (resultType)
            {
                case QueryResultType.Double:
                    await reader.ReadAsync(cancellationToken).ConfigureAwait(false);
                    isDbNull = await reader.IsDBNullAsync(0, cancellationToken).ConfigureAwait(false);
                    return isDbNull ? null : reader.GetDouble(0).ToString(CultureInfo.CurrentCulture);
                case QueryResultType.Json:
                    return await reader.ReadToJsonAsync().ConfigureAwait(false);
                case QueryResultType.String:
                    await reader.ReadAsync(cancellationToken).ConfigureAwait(false);
                    isDbNull = await reader.IsDBNullAsync(0, cancellationToken).ConfigureAwait(false);
                    return isDbNull ? null : reader.GetString(0);
                default:
                    return null;
            }
        }
    }

    private async Task<TResult> MakeQueryAndHandleResult<TResult>(
        string sqlQuery, Func<DbDataReader, Task<TResult>> handleResult, CancellationToken cancellationToken)
    {
        try
        {
            ArgumentException.ThrowIfNullOrEmpty(sqlQuery);

            var sw = Stopwatch.StartNew();
            await using var connection = new TConnection();
            {
                connection.ConnectionString = _connectionString;
                await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
                TResult result;

                await using var command = new TCommand();
                {
                    command.CommandText = sqlQuery;
                    command.Connection = connection;

                    await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
                    result = await handleResult.Invoke(reader);
                }

                sw.Stop();
                _logger?.LogDebug("{MethodName} [Elapsed: {Elapsed}].\n\tSQL: {SQL}", nameof(GetQueryResultAsync), sw.Elapsed, sqlQuery);

                await connection.CloseAsync().ConfigureAwait(false);
                return result;
            }
        }
        catch (InvalidCastException ex)
        {
            _logger?.LogError(ex, "Error getting query result");
            throw;
        }
    }
}