using System.Threading.Tasks;

namespace Zs.Common.Abstractions.Data;

public interface IItemsWithRawDataRepository<TEntity, TKey> : IRepository<TEntity, TKey>
    where TEntity : class, IDbEntityWithRawData<TEntity, TKey>
{
    Task<TKey> GetActualIdByRawDataHashAsync(TEntity item);
}
