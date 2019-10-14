using Core.DomainModel.Entities;
using Core.DomainService.Repository;
using System.Threading.Tasks;

namespace Infrastructure.DataBase.Repository
{
    public abstract class BaseRepository<TEntity, TKey> : BaseReadOnlyRepository<TEntity, TKey>, IBaseRepository<TEntity, TKey>
           where TEntity : BaseEntity
    {

        #region Constructors

        public BaseRepository(ManufacturingDbContext dbContext) :
            base(dbContext)
        {
        }

        #endregion /Constructors

        #region Methods

        public virtual async Task InsertAsync(TEntity entity)
        {
            await base.DBContext.AddAsync(entity);
        }

        #endregion /Methods

    }
}
