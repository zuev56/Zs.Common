using System.Threading.Tasks;
using Zs.Common.Enums;

namespace Zs.Common.Abstractions.Data;

public interface IDbClient
{
    Task<string> GetQueryResultAsync(string sqlQuery);
    Task<string> GetQueryResultAsync(string sqlQuery, QueryResultType resultType);
}