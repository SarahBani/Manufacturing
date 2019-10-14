using Core.ApplicationService.Contracts;
using Core.ApplicationService.Implementation;
using Core.DomainModel.Entities;
using Core.DomainService;
using Core.DomainService.Repository;
using System.Linq;

namespace Core.ApplicationService
{
    public class EntityService : IEntityService
    {

        #region Properties

        public IUnitOfWork UnitOfWork { get; private set; }

        #region Repositories

        public IOrderRepository OrderRepository { get; private set; }

        public IOrderDetailRepository OrderDetailRepository { get; private set; }

        public IProductTypeRepository ProductTypeRepository { get; private set; }

        #endregion /Repositories

        #region Services

        private IOrderService _orderService;
        public IOrderService OrderService
        {
            get
            {
                if (_orderService == null)
                {
                    _orderService = new OrderService(this);

                }
                return _orderService;
            }
        }

        private IOrderDetailService _orderDetailService;
        public IOrderDetailService OrderDetailService
        {
            get
            {
                if (_orderDetailService == null)
                {
                    _orderDetailService = new OrderDetailService(this);

                }
                return _orderDetailService;
            }
        }

        private IProductTypeService _productTypeService;
        public IProductTypeService ProductTypeService
        {
            get
            {
                if (_productTypeService == null)
                {
                    _productTypeService = new ProductTypeService(this);

                }
                return _productTypeService;
            }
        }

        #endregion /Repositories

        #endregion /Properties

        #region Constructors

        public EntityService(IBaseRepository<Order, long> orderRepository,
            IBaseRepository<OrderDetail, long> orderDetailRepository,
            IBaseReadOnlyRepository<ProductType, byte> productTypeRepository,
            IUnitOfWork unitOfWork)
        {
            this.OrderRepository = (orderRepository as IOrderRepository);
            this.OrderDetailRepository = (orderDetailRepository as IOrderDetailRepository);
            this.ProductTypeRepository = (productTypeRepository as IProductTypeRepository);

            this.UnitOfWork = unitOfWork;
        }

        #endregion /Constructors

        #region Methods

        public IBaseReadOnlyRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
            where TEntity : BaseEntity
        {
            string entityName = typeof(TEntity).Name;

            var prop = this.GetType().GetProperties()
                .Where(q => q.Name.Equals(entityName + "Repository"))
                .SingleOrDefault();

            return prop.GetValue(this) as IBaseReadOnlyRepository<TEntity, TKey>;
        }

        #endregion /Methods

    }
}
