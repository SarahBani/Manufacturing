using Core.DomainModel.Entities;
using Core.DomainService.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public virtual async Task<TEntity> GetByIdAsync(TKey id)
        {
            return await this.DBContext.Set<TEntity>().FindAsync(id);
        }

        public virtual async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await Task.Run(() => this.DBContext.Set<TEntity>()
                .Where(filter)
                .SingleOrDefault());
        }

        public virtual async Task<IQueryable<TEntity>> GetQueryableAsync()
        {
            return await Task.Run(() => this.DBContext.Set<TEntity>().AsQueryable());
        }

        public virtual async  Task<IEnumerable<TEntity>> GetEnumerableAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            return await Task.Run(() => this.DBContext.Set<TEntity>().Where(filter));
        }

        #endregion /Methods

    }
}
