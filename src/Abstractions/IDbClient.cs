using System.Threading.Tasks;
using Zs.Common.Enums;

namespace Zs.Common.Abstractions
{
    public interface IDbClient
    {
        Task<string> GetQueryResultAsync(string sqlQuery);
        Task<string> GetQueryResultAsync(string sqlQuery, QueryResultType resultType);
    }
}
