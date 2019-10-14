using Core.DomainModel.Entities;
using System.Collections.Generic;
using WebAPI;

namespace Test.Common.Models
{
    public class OrderModel : BaseModel<Order>
    {

        public Order Entity
        {
            get => new Order()
            {
                OrderID = 5,
                OrderDetails = new OrderDetailModel().EntityList
            };
        }

        public IList<Order> EntityList
        {
            get => new List<Order>
                {
                    new Order()
                    {
                        OrderID = 3,
                        OrderDetails = new OrderDetailModel().EntityList
                    }
                };
        }

        #region OrderItems

        public IDictionary<string, short> OrderItems_Simple
        {
            get => new Dictionary<string, short>
                {
                    {"cards", 4 },
                };
        }

        public float RequiredBinWidth_Simple
        {
            get => 4 * 4.7f;
        }

        public IDictionary<string, short> OrderItems_Sample1
        {
            get => new Dictionary<string, short>
                {
                    {"photoBook", 1 },
                    {"calendar", 2 },
                    {"mug", 1 },
                };
        }

        public IDictionary<string, short> OrderItems_Sample2
        {
            get => new Dictionary<string, short>
                {
                    {"photoBook", 1 },
                    {"calendar", 2 },
                    {"mug", 2 }, // 1 stack
                };
        }

        public float RequiredBinWidth_Sample1And2
        {
            get => (1 * 19) + (2 * 10) + (1 * 94);
        }

        public IDictionary<string, short> OrderItems_Sample3
        {
            get => new Dictionary<string, short>
                {
                    {"photoBook", 1 },
                    {"calendar", 2 },
                    {"mug", 5 }, // 2 stacks
                };
        }

        public float RequiredBinWidth_Sample3
        {
            get => (1 * 19) + (2 * 10) + (2 * 94);
        }

        public IDictionary<string, short> OrderItems_Complex
        {
            get => new Dictionary<string, short>
                {
                    {"cards", 2 },
                    {"mug", 9 }, // 3 stacks
                    {"canvas", 1 },
                    {"photoBook", 4 }
                };
        }

        public float RequiredBinWidth_Complex
        {
            get => (2 * 4.7f) + (3 * 94) + (1 * 16) + (4 * 19);
        }

        public IDictionary<string, short> OrderItems_WithOneZeroQuantity
        {
            get => new Dictionary<string, short>
                {
                    {"calendar", 2 },
                    {"canvas", 0 },
                    {"photoBook", 4 }
                };
        }

        public float RequiredBinWidth_WithOneZeroQuantity
        {
            get => (2 * 10) + (0 * 16) + (4 * 19);
        }

        public IDictionary<string, short> OrderItems_WithoutAnyQuantity
        {
            get => new Dictionary<string, short>
                {
                    {"cards", 0 },
                    {"mug", 0 },
                    {"canvas", 0 }
                };
        }

        public IDictionary<string, short> OrderItems_WithInvalidProductType
        {
            get => new Dictionary<string, short>
                {
                    {"mug", 9 },
                    {"xxx", 1 }, // invalid
                    {"photoBook", 4 }
                };
        }

        public IDictionary<string, short> OrderItems_WithInvalidQuantity
        {
            get => new Dictionary<string, short>
                {
                    {"mug", 9 },
                    {"canvas", -1 }, // invalid Quantity
                    {"photoBook", 4 }
                };
        }

        #endregion /OrderItems

        #region CustomerOrder

        public CustomerOrder CustomerOrder_Simple
        {
            get => new CustomerOrder()
            {
                OrderID = 5,
                OrderItems = this.OrderItems_Simple
            };
        }

        public CustomerOrder CustomerOrder_Sample1
        {
            get => new CustomerOrder()
            {
                OrderID = 5,
                OrderItems = this.OrderItems_Sample1
            };
        }

        public CustomerOrder CustomerOrder_Sample2
        {
            get => new CustomerOrder()
            {
                OrderID = 5,
                OrderItems = this.OrderItems_Sample2
            };
        }

        public CustomerOrder CustomerOrder_Sample3
        {
            get => new CustomerOrder()
            {
                OrderID = 5,
                OrderItems = this.OrderItems_Sample3
            };
        }

        public CustomerOrder CustomerOrder_Complex
        {
            get => new CustomerOrder()
            {
                OrderID = 5,
                OrderItems = this.OrderItems_Complex
            };
        }

        public CustomerOrder CustomerOrder_WithOneZeroQuantity
        {
            get => new CustomerOrder()
            {
                OrderID = 5,
                OrderItems = this.OrderItems_WithOneZeroQuantity
            };
        }

        public CustomerOrder CustomerOrder_WithInvalidOrderID
        {
            get => new CustomerOrder()
            {
                OrderID = -1,
                OrderItems = this.OrderItems_Simple
            };
        }

        public CustomerOrder CustomerOrder_WithNullOrderItems
        {
            get => new CustomerOrder()
            {
                OrderID = 5,
                OrderItems = null
            };
        }

        public CustomerOrder CustomerOrder_WithoutAnyQuantity
        {
            get => new CustomerOrder()
            {
                OrderID = 5,
                OrderItems = this.OrderItems_WithoutAnyQuantity
            };
        }

        public CustomerOrder CustomerOrder_WithInvalidProductType
        {
            get => new CustomerOrder()
            {
                OrderID = 5,
                OrderItems = this.OrderItems_WithInvalidProductType
            };
        }

        public CustomerOrder CustomerOrder_WithInvalidQuantity
        {
            get => new CustomerOrder()
            {
                OrderID = 5,
                OrderItems = this.OrderItems_WithInvalidQuantity
            };
        }

        #endregion /CustomerOrder

    }
}
