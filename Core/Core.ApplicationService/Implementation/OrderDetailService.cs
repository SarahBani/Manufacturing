using Core.ApplicationService.Contracts;
using Core.DomainModel.Entities;
using Core.DomainService.Repository;
using System.Collections.Generic;
using System.Linq;

namespace Core.ApplicationService.Implementation
{
    public class OrderDetailService : BaseService<IOrderDetailRepository, OrderDetail, long>, IOrderDetailService
    {

        #region Constructors

        public OrderDetailService(IEntityService entityService)
            : base(entityService)
        {
        }

        #endregion /Constructors

        #region Methods

        public IList<OrderDetail> GetListByOrderID(long orderID)
        {
            return base.GetEnumerable(q => q.OrderID.Equals(orderID)).ToList();
        }

        #endregion /Methods

    }
}
