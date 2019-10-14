using Core.DomainModel.Entities;
using Core.DomainService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.ApplicationService.Contracts
{
    public interface IOrderDetailService
    {

        Task<IList<OrderDetail>> GetListByOrderIDAsync(long orderID);

        Task<TransactionResult> InsertAsync(OrderDetail orderDetail);

    }
}
