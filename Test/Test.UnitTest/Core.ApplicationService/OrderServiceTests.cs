using Core.ApplicationService.Implementation;
using Core.DomainModel;
using Core.DomainModel.Entities;
using Core.DomainService;
using Core.DomainService.Repository;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Test.Common;
using Test.Common.Models;
using System.Threading.Tasks;

namespace Test.UnitTest.Core.ApplicationService
{
    [TestFixture]
    public class OrderServiceTests : BaseServiceTests<IOrderRepository, Order, long>
    {

        #region Properties

        protected new OrderService Service
        {
            get => base.Service as OrderService;
        }

        protected override Order Entity
        {
            get => new OrderModel().Entity;
        }

        protected override IList<Order> EntityList
        {
            get => new OrderModel().EntityList;
        }

        #endregion /Properties

        #region Constructors

        public OrderServiceTests()
            : base()
        {
        }

        #endregion /Constructors

        #region Methods

        [OneTimeSetUp]
        public override void Setup()
        {
            base.SetService<OrderService>();
        }

        #region GetByOrderIDAsync

        [Test]
        public async Task GetByOrderIDAsync_ReturnsOK()
        {
            // Arrange
            long orderID = 5;
            var order = this.Entity;
            base.RepositoryMock.Setup(q => q.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(order);

            //Act
            var result = await this.Service.GetByOrderIDAsync(orderID);

            // Assert
            this.RepositoryMock.Verify(q => q.GetByIdAsync(orderID),
                "error in calling the correct method");  // Verifies that Repository.GetByIdAsync was called
            Assert.AreEqual(order, result, "error in returning correct entity");
        }

        [Test]
        public async Task GetByOrderIDAsync_IdNotExist_ReturnsNull()
        {
            // Arrange
            long orderID = 0;
            Order order = null;
            base.RepositoryMock.Setup(q => q.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(order);

            //Act
            var result = await this.Service.GetByOrderIDAsync(orderID);

            // Assert
            this.RepositoryMock.Verify(q => q.GetByIdAsync(orderID),
                "error in calling the correct method");  // Verifies that Repository.GetByIdAsync was called
            Assert.IsNull(result, "error in returning null entity");
        }

        #endregion /GetByOrderIDAsync

        #region GetMaxOrderIDAsync

        [Test]
        public async Task GetMaxOrderIDAsync_NoOrdersYet_ReturnsOK()
        {
            // Arrange
            var orders = new List<Order>();
            long maxOrderID = 0;
            base.RepositoryMock.Setup(q => q.GetQueryableAsync()).ReturnsAsync(orders.AsQueryable());

            //Act
            var result = await this.Service.GetMaxOrderIDAsync();

            // Assert
            this.RepositoryMock.Verify(q => q.GetQueryableAsync(),
                "error in calling the correct method");  // Verifies that Repository.GetQueryableAsync was called
            Assert.AreEqual(maxOrderID, result, "error in returning correct OrderID");
        }

        [Test]
        public async Task GetMaxOrderIDAsync_ReturnsOK()
        {
            // Arrange
            var orders = this.EntityList;
            long maxOrderID = this.EntityList.Max(q => q.OrderID);
            base.RepositoryMock.Setup(q => q.GetQueryableAsync()).ReturnsAsync(orders.AsQueryable());

            //Act
            var result = await this.Service.GetMaxOrderIDAsync();

            // Assert
            this.RepositoryMock.Verify(q => q.GetQueryableAsync(),
                "error in calling the correct method");  // Verifies that Repository.GetQueryableAsync was called
            Assert.AreEqual(maxOrderID, result, "error in returning correct OrderID");
        }

        #endregion /GetMaxOrderIDAsync

        #region SaveOrderAsync   

        [Test]
        public async Task SaveOrderAsync_InvalidOrderID_ReturnsInvalidOrderID()
        {
            // Arrange
            long orderID = -1;
            var orderItems = new OrderModel().OrderItems_Simple;
            var expectedResult = new TransactionResult(new CustomException(ExceptionKey.InvalidOrderID));

            //Act
            var result = await this.Service.SaveOrderAsync(orderID, orderItems);

            // Assert
            Assert.IsInstanceOf<TransactionResult>(result, "error in returning correct result type");
            TestHelper.AreEqualEntities(expectedResult, result, "error in returning correct TransactionResult");
        }

        [Test]
        public async Task SaveOrderAsync_NullOrderItems_ReturnsOrderItemsNotExist()
        {
            // Arrange
            long orderID = 5;
            IDictionary<string, short> orderItems = null;
            var expectedResult = new TransactionResult(new CustomException(ExceptionKey.OrderItemsNotExist));

            //Act
            var result = await this.Service.SaveOrderAsync(orderID, orderItems);

            // Assert
            Assert.IsInstanceOf<TransactionResult>(result, "error in returning correct result type");
            TestHelper.AreEqualEntities(expectedResult, result, "error in returning correct TransactionResult");
        }

        [Test]
        public async Task SaveOrderAsync_OrderItemsWithoutAnyQuantity_ReturnsOrderItemsNotExist()
        {
            // Arrange
            long orderID = 5;
            var orderItems = new OrderModel().OrderItems_WithoutAnyQuantity;
            var expectedResult = new TransactionResult(new CustomException(ExceptionKey.OrderItemsNotExist));

            //Act
            var result = await this.Service.SaveOrderAsync(orderID, orderItems);

            // Assert
            Assert.IsInstanceOf<TransactionResult>(result, "error in returning correct result type");
            TestHelper.AreEqualEntities(expectedResult, result, "error in returning correct TransactionResult");
        }

        [Test]
        public async Task SaveOrderAsync_OrderItemsWithInvalidQuantity_ReturnsInvalidOrderItemsQuantity()
        {
            // Arrange
            long orderID = 5;
            var orderItems = new OrderModel().OrderItems_WithInvalidQuantity;
            var expectedResult = new TransactionResult(new CustomException(ExceptionKey.InvalidOrderItemsQuantity));

            //Act
            var result = await this.Service.SaveOrderAsync(orderID, orderItems);

            // Assert
            Assert.IsInstanceOf<TransactionResult>(result, "error in returning correct result type");
            TestHelper.AreEqualEntities(expectedResult, result, "error in returning correct TransactionResult");
        }

        [Test]
        public async Task SaveOrderAsync_OrderItemsWithInvalidProductType_ReturnsInvalidOrderItemsProductType()
        {
            // Arrange
            long orderID = 5;
            var orderItems = new OrderModel().OrderItems_WithInvalidProductType;
            var productTypes = new ProductTypeModel().EntityList;
            var expectedResult = new TransactionResult(new CustomException(ExceptionKey.InvalidOrderItemsProductType));

            base.EntityServiceMock.Setup(q => q.ProductTypeService.GetAllAsync()).ReturnsAsync(productTypes);

            //Act
            var result = await this.Service.SaveOrderAsync(orderID, orderItems);

            // Assert
            Assert.IsInstanceOf<TransactionResult>(result, "error in returning correct result type");
            TestHelper.AreEqualEntities(expectedResult, result, "error in returning correct TransactionResult");
        }

        [Test]
        public async Task SaveOrderAsync_ProductTypeGetAllAsync_ReturnsOK()
        {
            // Arrange
            var order = this.Entity;
            var orderItems = new OrderModel().OrderItems_Simple;
            var productTypes = new ProductTypeModel().EntityList;

            base.EntityServiceMock.Setup(q => q.ProductTypeService.GetAllAsync()).ReturnsAsync(productTypes);

            //Act
            var result = await this.Service.SaveOrderAsync(order.OrderID, orderItems);

            // Assert
            Assert.IsInstanceOf<TransactionResult>(result, "error in returning correct result type");
            base.EntityServiceMock.Verify(q => q.ProductTypeService.GetAllAsync(),
                "error in calling the ProductTypeService GetAllAsync method");  // Verifies that ProductTypeService.GetAllAsync was called
        }

        [Test]
        public async Task SaveOrderAsync_InsertOrder_ReturnsOK()
        {
            // Arrange
            var order = this.Entity;
            var orderItems = new OrderModel().OrderItems_Simple;
            var productTypes = new ProductTypeModel().EntityList;

            base.EntityServiceMock.Setup(q => q.ProductTypeService.GetAllAsync()).ReturnsAsync(productTypes);
            base.RepositoryMock.Setup(q => q.InsertAsync(order)).Verifiable();

            //Act
            var result = await this.Service.SaveOrderAsync(order.OrderID, orderItems);

            // Assert
            Assert.IsInstanceOf<TransactionResult>(result, "error in returning correct result type");
            base.RepositoryMock.Verify(q => q.InsertAsync(It.IsAny<Order>()),
                "error in calling the Repository InsertAsync method");  // Verifies that Repository.InsertAsync was called
        }

        [Test]
        public async Task SaveOrderAsync_SaveOrderDetails_Simple_ReturnsOK()
        {
            // Arrange
            var orderID = 5;
            var orderItems = new OrderModel().OrderItems_Simple;
            var productTypes = new ProductTypeModel().EntityList;
            var successfulResult = new TransactionResult();

            base.EntityServiceMock.Setup(q => q.ProductTypeService.GetAllAsync()).ReturnsAsync(productTypes);
            var dictOrderItems = orderItems.Where(q => q.Value > 0) // if quantity equals to 0, we neglect it
                .ToDictionary(q => productTypes.Single(x => x.Name.ToLower().Equals(q.Key.ToLower())), q => q.Value);
            base.EntityServiceMock.Setup(q => q.OrderDetailService.InsertAsync(It.IsAny<OrderDetail>())).ReturnsAsync(successfulResult);

            foreach (var item in dictOrderItems)
            {
                var productType = item.Key;
                var orderDetail = new OrderDetail()
                {
                    OrderID = orderID,
                    ProductTypeID = productType.ProductTypeID,
                    Quantity = item.Value
                };

                //Act
                var result = await this.Service.SaveOrderAsync(orderID, orderItems);

                // Assert
                Assert.IsInstanceOf<TransactionResult>(result, "error in returning correct result type");
                base.EntityServiceMock.Verify(q => q.OrderDetailService.InsertAsync(It.IsAny<OrderDetail>()),
                    "error in calling the correct method");  // Verifies that OrderDetailServices.InsertAsync was called
                Assert.AreEqual(true, result.IsSuccessful, "error in returning correct TransactionResult");
            }
        }

        [Test]
        public async Task SaveOrderAsync_SaveOrderDetails_Complex_ReturnsOK()
        {
            // Arrange
            var orderID = 5;
            var orderItems = new OrderModel().OrderItems_Complex;
            var productTypes = new ProductTypeModel().EntityList;
            var dictOrderItems = orderItems.Where(q => q.Value > 0) // if quantity equals to 0, we neglect it
                .ToDictionary(q => productTypes.Single(x => x.Name.ToLower().Equals(q.Key.ToLower())), q => q.Value);
            var successfulResult = new TransactionResult();

            base.EntityServiceMock.Setup(q => q.ProductTypeService.GetAllAsync()).ReturnsAsync(productTypes);
            base.EntityServiceMock.Setup(q => q.OrderDetailService.InsertAsync(It.IsAny<OrderDetail>())).ReturnsAsync(successfulResult);

            //Act
            var result = await this.Service.SaveOrderAsync(orderID, orderItems);

            // Assert
            Assert.IsInstanceOf<TransactionResult>(result, "error in returning correct result type");
            base.EntityServiceMock.Verify(q => q.OrderDetailService.InsertAsync(It.IsAny<OrderDetail>()),
                "error in calling the correct method");  // Verifies that OrderDetailServices.InsertAsync was called
            Assert.AreEqual(true, result.IsSuccessful, "error in returning correct TransactionResult");
        }

        [Test]
        public async Task SaveOrderAsync_RequiredBinWidthForOrderItemSimple_ReturnsOK()
        {
            // Arrange
            var orderID = 5;
            var orderItems = new OrderModel().OrderItems_Simple;
            float requiredBinWidth = new OrderModel().RequiredBinWidth_Simple;
            var productTypes = new ProductTypeModel().EntityList;
            var dictOrderItems = orderItems.Where(q => q.Value > 0) // if quantity equals to 0, we neglect it
                .ToDictionary(q => productTypes.Single(x => x.Name.ToLower().Equals(q.Key.ToLower())), q => q.Value);
            var successfulResult = new TransactionResult();

            base.EntityServiceMock.Setup(q => q.ProductTypeService.GetAllAsync()).ReturnsAsync(productTypes);
            base.EntityServiceMock.Setup(q => q.OrderDetailService.InsertAsync(It.IsAny<OrderDetail>())).ReturnsAsync(successfulResult);

            //Act
            var result = await this.Service.SaveOrderAsync(orderID, orderItems);

            // Assert
            Assert.IsInstanceOf<TransactionResult>(result, "error in returning correct result type");
            Assert.AreEqual(requiredBinWidth, result.Content, "error in returning correct RequiredBinWidth");
        }

        [Test]
        public async Task SaveOrderAsync_RequiredBinWidthForOrderItemSample1_ReturnsOK()
        {
            // Arrange
            var orderID = 5;
            var orderItems = new OrderModel().OrderItems_Sample1;
            float requiredBinWidth = new OrderModel().RequiredBinWidth_Sample1And2;
            var productTypes = new ProductTypeModel().EntityList;
            var dictOrderItems = orderItems.Where(q => q.Value > 0) // if quantity equals to 0, we neglect it
                .ToDictionary(q => productTypes.Single(x => x.Name.ToLower().Equals(q.Key.ToLower())), q => q.Value);
            var successfulResult = new TransactionResult();

            base.EntityServiceMock.Setup(q => q.ProductTypeService.GetAllAsync()).ReturnsAsync(productTypes);
            base.EntityServiceMock.Setup(q => q.OrderDetailService.InsertAsync(It.IsAny<OrderDetail>())).ReturnsAsync(successfulResult);

            //Act
            var result = await this.Service.SaveOrderAsync(orderID, orderItems);

            // Assert
            Assert.IsInstanceOf<TransactionResult>(result, "error in returning correct result type");
            Assert.AreEqual(requiredBinWidth, result.Content, "error in returning correct RequiredBinWidth");
        }

        [Test]
        public async Task SaveOrderAsync_RequiredBinWidthForOrderItemSample2_ReturnsOK()
        {
            // Arrange
            var orderID = 5;
            var orderItems = new OrderModel().OrderItems_Sample2;
            float requiredBinWidth = new OrderModel().RequiredBinWidth_Sample1And2;
            var productTypes = new ProductTypeModel().EntityList;
            var dictOrderItems = orderItems.Where(q => q.Value > 0) // if quantity equals to 0, we neglect it
                .ToDictionary(q => productTypes.Single(x => x.Name.ToLower().Equals(q.Key.ToLower())), q => q.Value);
            var successfulResult = new TransactionResult();

            base.EntityServiceMock.Setup(q => q.ProductTypeService.GetAllAsync()).ReturnsAsync(productTypes);
            base.EntityServiceMock.Setup(q => q.OrderDetailService.InsertAsync(It.IsAny<OrderDetail>())).ReturnsAsync(successfulResult);

            //Act
            var result = await this.Service.SaveOrderAsync(orderID, orderItems);

            // Assert
            Assert.IsInstanceOf<TransactionResult>(result, "error in returning correct result type");
            Assert.AreEqual(requiredBinWidth, result.Content, "error in returning correct RequiredBinWidth");
        }

        [Test]
        public async Task SaveOrderAsync_RequiredBinWidthForOrderItemSample3_ReturnsOK()
        {
            // Arrange
            var orderID = 5;
            var orderItems = new OrderModel().OrderItems_Sample3;
            float requiredBinWidth = new OrderModel().RequiredBinWidth_Sample3;
            var productTypes = new ProductTypeModel().EntityList;
            var dictOrderItems = orderItems.Where(q => q.Value > 0) // if quantity equals to 0, we neglect it
                .ToDictionary(q => productTypes.Single(x => x.Name.ToLower().Equals(q.Key.ToLower())), q => q.Value);
            var successfulResult = new TransactionResult();

            base.EntityServiceMock.Setup(q => q.ProductTypeService.GetAllAsync()).ReturnsAsync(productTypes);
            base.EntityServiceMock.Setup(q => q.OrderDetailService.InsertAsync(It.IsAny<OrderDetail>())).ReturnsAsync(successfulResult);

            //Act
            var result = await this.Service.SaveOrderAsync(orderID, orderItems);

            // Assert
            Assert.IsInstanceOf<TransactionResult>(result, "error in returning correct result type");
            Assert.AreEqual(requiredBinWidth, result.Content, "error in returning correct RequiredBinWidth");
        }

        [Test]
        public async Task SaveOrderAsync_RequiredBinWidthForOrderItemComplex_ReturnsOK()
        {
            // Arrange
            var orderID = 5;
            var orderItems = new OrderModel().OrderItems_Complex;
            float requiredBinWidth = new OrderModel().RequiredBinWidth_Complex;
            var productTypes = new ProductTypeModel().EntityList;
            var dictOrderItems = orderItems.Where(q => q.Value > 0) // if quantity equals to 0, we neglect it
                .ToDictionary(q => productTypes.Single(x => x.Name.ToLower().Equals(q.Key.ToLower())), q => q.Value);
            var successfulResult = new TransactionResult();

            base.EntityServiceMock.Setup(q => q.ProductTypeService.GetAllAsync()).ReturnsAsync(productTypes);
            base.EntityServiceMock.Setup(q => q.OrderDetailService.InsertAsync(It.IsAny<OrderDetail>())).ReturnsAsync(successfulResult);

            //Act
            var result = await this.Service.SaveOrderAsync(orderID, orderItems);

            // Assert
            Assert.IsInstanceOf<TransactionResult>(result, "error in returning correct result type");
            Assert.AreEqual(requiredBinWidth, result.Content, "error in returning correct RequiredBinWidth");
        }

        [Test]
        public async Task SaveOrderAsync_RequiredBinWidthForOrderItemWithOneZeroQuantity_ReturnsOK()
        {
            // Arrange
            var orderID = 5;
            var orderItems = new OrderModel().OrderItems_WithOneZeroQuantity;
            float requiredBinWidth = new OrderModel().RequiredBinWidth_WithOneZeroQuantity;
            var productTypes = new ProductTypeModel().EntityList;
            var dictOrderItems = orderItems.Where(q => q.Value > 0) // if quantity equals to 0, we neglect it
                .ToDictionary(q => productTypes.Single(x => x.Name.ToLower().Equals(q.Key.ToLower())), q => q.Value);
            var successfulResult = new TransactionResult();

            base.EntityServiceMock.Setup(q => q.ProductTypeService.GetAllAsync()).ReturnsAsync(productTypes);
            base.EntityServiceMock.Setup(q => q.OrderDetailService.InsertAsync(It.IsAny<OrderDetail>())).ReturnsAsync(successfulResult);

            //Act
            var result = await this.Service.SaveOrderAsync(orderID, orderItems);

            // Assert
            Assert.IsInstanceOf<TransactionResult>(result, "error in returning correct result type");
            Assert.AreEqual(requiredBinWidth, result.Content, "error in returning correct RequiredBinWidth");
        }

        #endregion /SaveOrderAsync   

        #endregion /Methods

    }
}
