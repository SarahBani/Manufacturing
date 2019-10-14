using Core.DomainModel.Entities;
using System.Collections.Generic;

namespace Test.Common.Models
{
    public class OrderDetailModel : BaseModel<OrderDetail>
    {

        public OrderDetail Entity
        {
            get => new OrderDetail()
            {
                OrderDetailID = 3,
                OrderID = 5,
                ProductTypeID = 3,
                Quantity = 3
            };
        }

        public IList<OrderDetail> EntityList
        {
            get => new List<OrderDetail>
                {
                    new OrderDetail()
                    {
                        OrderDetailID = 1,
                        OrderID = 5,
                        ProductTypeID = 1,
                        Quantity = 3
                    },
                    new OrderDetail()
                    {
                        OrderDetailID = 2,
                        OrderID = 5,
                        ProductTypeID = 3,
                        Quantity = 4
                    },
                    new OrderDetail()
                    {
                        OrderDetailID = 3,
                        OrderID = 5,
                        ProductTypeID = 5,
                        Quantity = 9
                    },
                };
        }

    }
}
