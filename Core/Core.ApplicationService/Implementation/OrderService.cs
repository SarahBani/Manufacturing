using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.ApplicationService.Contracts;
using Core.DomainModel;
using Core.DomainModel.Entities;
using Core.DomainService;
using Core.DomainService.Repository;

namespace Core.ApplicationService.Implementation
{
    public class OrderService : BaseService<IOrderRepository, Order, long>, IOrderService
    {

        #region Constructors

        public OrderService(IEntityService entityService)
            : base(entityService)
        {
        }

        #endregion /Constructors

        #region Methods

        public Task<Order> GetByOrderIDAsync(long orderID)
        {
            return base.GetByIdAsync(orderID);
        }

        /// <summary>
        /// occording to the document OrderID should be specified by user
        /// so, I added this method for getting the last OrderID for testing
        /// </summary>
        /// <returns></returns>
        public long GetMaxOrderID()
        {
            var query = base.GetQueryable();
            if (query.Any())
            {
                return query.OrderByDescending(q => q.OrderID)
                    .Select(q => q.OrderID)
                    .First();
            }
            else
            {
                return 0;
            }
        }

        public async Task<TransactionResult> SaveOrderAsync(long orderID, IDictionary<string, short> orderItems)
        {
            try
            {
                if (orderID <= 0)
                {
                    throw new CustomException(ExceptionKey.InvalidOrderID);
                }
                if (orderItems == null || !orderItems.Any(q => q.Value > 0))
                {
                    throw new CustomException(ExceptionKey.OrderItemsNotExist);
                }
                if (orderItems.Any(q => q.Value < 0))
                {
                    throw new CustomException(ExceptionKey.InvalidOrderItemsQuantity);
                }
                var productTypes = base.EntityService.ProductTypeService.GetAll();
                if (orderItems.Any(q => !productTypes.Any(x => x.Name.ToLower().Equals(q.Key.ToLower()))))
                {
                    throw new CustomException(ExceptionKey.InvalidOrderItemsProductType);
                }
                var dictOrderItems = orderItems.Where(q => q.Value > 0) // if quantity equals to 0, we neglect it
                    .ToDictionary(q => productTypes.Single(x => x.Name.ToLower().Equals(q.Key.ToLower())), q => q.Value);

                float requiredBinWidth = GetRequiredBinWidth(dictOrderItems);
                var order = new Order()
                {
                    OrderID = orderID,
                    RequiredBinWidth = requiredBinWidth
                };
                var orderDetails = new List<OrderDetail>();
                BeginTransaction();
                var transactionResult = await base.InsertAsync(order);
                if (!transactionResult.IsSuccessful)
                {
                    return transactionResult;
                }
                transactionResult = await SaveOrderDetails(order.OrderID, dictOrderItems);
                if (!transactionResult.IsSuccessful)
                {
                    return transactionResult;
                }
                return await CommitTransactionAsync(requiredBinWidth);
            }
            catch (Exception ex)
            {
                return GetTransactionException(ex);
            }
        }

        private float GetRequiredBinWidth(Dictionary<ProductType, short> dictOrderItems)
        {
            return dictOrderItems.Sum(q =>
            {
                var productType = q.Key;
                int placeCount = (q.Value + productType.StackedCount - 1) / productType.StackedCount;
                return (placeCount * productType.Width);
            });
        }

        private async Task<TransactionResult> SaveOrderDetails(long orderID, Dictionary<ProductType, short> dictOrderItems)
        {
            var transactionResult = new TransactionResult();
            var tasks = new List<Task<TransactionResult>>();
            foreach (var item in dictOrderItems)
            {
                var productType = item.Key;
                var orderDetail = new OrderDetail()
                {
                    OrderID = orderID,
                    ProductTypeID = productType.ProductTypeID,
                    Quantity = item.Value
                };
                tasks.Add(base.EntityService.OrderDetailService.InsertAsync(orderDetail)); // they can be inserted asyncronously
            }
            await Task.WhenAll(tasks.ToArray()).ContinueWith(action =>
            {
                if (action.Result.Any(q => !q.IsSuccessful))
                {
                    transactionResult = action.Result.First(q => !q.IsSuccessful);
                }
            });
            return transactionResult;
        }

        #endregion /Methods

    }
}
