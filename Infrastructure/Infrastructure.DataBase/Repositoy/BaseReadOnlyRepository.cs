using Core.DomainModel.Entities;
using Core.DomainService.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.DataBase.Repository
{
    public abstract class BaseReadOnlyRepository<TEntity, TKey> : IBaseReadOnlyRepository<TEntity, TKey>
        where TEntity : BaseEntity
    {

        #region Properties

        protected readonly ManufacturingDbContext DBContext;

        #endregion /Properties

        #region Constructors

        public BaseReadOnlyRepository(ManufacturingDbContext dbContext)
        {
            this.DBContext = dbContext;
        }

        #endregion /Constructors

        #region Methods
        
        public virtual Task<TEntity> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return this.DBContext.Set<TEntity>().FindAsync(id, cancellationToken);
        }

        public virtual TEntity GetSingle(Expression<Func<TEntity, bool>> filter)
        {
            return this.DBContext.Set<TEntity>().Where(filter).SingleOrDefault();
        }

        public virtual IQueryable<TEntity> GetQueryable()
        {
            return this.DBContext.Set<TEntity>().AsQueryable();
        }

        public virtual IEnumerable<TEntity> GetEnumerable(Expression<Func<TEntity, bool>> filter = null)
        {
            return this.DBContext.Set<TEntity>().Where(filter);
        }

        #endregion /Methods

    }
}
