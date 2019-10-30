using Core.DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.DomainService.Repository
{
    public interface IBaseReadOnlyRepository<TEntity, TKey>
        where TEntity : BaseEntity
    {

        Task<TEntity> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);

        TEntity GetSingle(Expression<Func<TEntity, bool>> filter);

        IQueryable<TEntity> GetQueryable();

        IEnumerable<TEntity> GetEnumerable(Expression<Func<TEntity, bool>> filter = null);

    }
}
