using Core.DomainModel.Entities;
using Core.DomainService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.ApplicationService.Contracts
{
    public interface IOrderService
    {

        Task<Order> GetByOrderIDAsync(long orderID);

        Task<long> GetMaxOrderIDAsync();

        Task<TransactionResult> SaveOrderAsync(long orderID, IDictionary<string, short> orderItems);

    }
}
