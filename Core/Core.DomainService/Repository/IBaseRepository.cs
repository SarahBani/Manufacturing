using Core.DomainModel.Entities;
using System.Threading.Tasks;

namespace Core.DomainService.Repository
{
    public interface IBaseRepository<TEntity, TKey> : IBaseReadOnlyRepository<TEntity, TKey>
        where TEntity : BaseEntity
    {

        Task InsertAsync(TEntity entity);

    }
}
