using Core.ApplicationService.Contracts;
using Core.DomainModel.Entities;
using Core.DomainService.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<IList<OrderDetail>> GetListByOrderIDAsync(long orderID)
        {
            return await Task.Run(() => base.GetEnumerableAsync(q => q.OrderID.Equals(orderID)).Result.ToList());
        }

        #endregion /Methods

    }
}
