using Core.ApplicationService.Contracts;
using Core.DomainModel.Entities;
using Core.DomainService;
using Core.DomainService.Repository;

namespace Core.ApplicationService
{
    public interface IEntityService
    {

        IUnitOfWork UnitOfWork { get; }

        IOrderRepository OrderRepository { get; }

        IOrderDetailRepository OrderDetailRepository { get; }

        IProductTypeRepository ProductTypeRepository { get; }

        IOrderService OrderService { get; }

        IOrderDetailService OrderDetailService { get; }

        IProductTypeService ProductTypeService { get; }

        IBaseReadOnlyRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
            where TEntity : BaseEntity;

    }
}
