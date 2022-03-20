using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Zs.Common.Abstractions.Data;

/// <summary>  </summary>
/// <typeparam name="TEntity">Search entity type</typeparam>
public interface IRepository<TEntity, TKey>
    where TEntity : class, IDbEntity<TEntity, TKey>
{
    Task<TEntity> FindByIdAsync(TKey id, CancellationToken cancellationToken = default);

    /// <summary>Returns the list of elements</summary>
    Task<List<TEntity>> FindAllAsync(int? skip = null, int? take = null, CancellationToken cancellationToken = default);

    Task<List<TEntity>> FindAllAsync(TKey[] keys, CancellationToken cancellationToken = default);

    /// <summary>Save new item or update existing item in database</summary>
    /// <param name="item"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>TRUE if saved or updated, otherwise FALSE</returns>
    Task<bool> SaveAsync(TEntity item, CancellationToken cancellationToken = default);

    /// <summary>Save new items range or update existing items in database</summary>
    /// <param name="items"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>TRUE if saved or updated, otherwise FALSE</returns>
    Task<bool> SaveRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken = default);

    /// <summary>Delete existing item from database</summary>
    /// <param name="item"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>TRUE if deleted, otherwise FALSE</returns>
    Task<bool> DeleteAsync(TEntity item, CancellationToken cancellationToken = default);

    /// <summary>Delete existing items range from database</summary>
    /// <param name="items"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>TRUE if the items deleted, otherwise FALSE</returns>
    Task<bool> DeleteRangeAsync(IEnumerable<TEntity> items, CancellationToken cancellationToken = default);
}
