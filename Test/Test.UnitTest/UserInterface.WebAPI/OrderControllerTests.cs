using Core.ApplicationService.Contracts;
using Core.DomainModel;
using Core.DomainModel.Entities;
using Core.DomainService;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Linq;
using Test.Common;
using Test.Common.Models;
using WebAPI;
using WebAPI.Controllers;
using System.Threading.Tasks;

namespace Test.UnitTest.UserInterface.WebAPI
{
    [TestFixture]
    public class OrderControllerTests
    {

        #region Properties

        private OrderController _controller;

        private Mock<IOrderService> _orderServiceMock;

        private Mock<IOrderDetailService> _orderDetailServiceMock;

        private Mock<IProductTypeService> _productTypeServiceMock;

        private Order Entity
        {
            get => new OrderModel().Entity;
        }

        #endregion /Properties

        #region Constructors

        public OrderControllerTests()
            : base()
        {
        }

        #endregion /Constructors

        #region Methods

        [OneTimeSetUp]
        public void Setup()
        {
            this._orderServiceMock = new Mock<IOrderService>();
            this._orderDetailServiceMock = new Mock<IOrderDetailService>();
            this._productTypeServiceMock = new Mock<IProductTypeService>();
            this._controller = new OrderController(_orderServiceMock.Object,
                _orderDetailServiceMock.Object,
                _productTypeServiceMock.Object);
        }

        #region GetAsync

        [Test]
        public async Task GetAsync_InvalidOrderID_ReturnsBadRequestOrderNotExist()
        {
            // Arrange
            long orderID = -1;
            Order order = null;
            var expectedResult = new BadRequestObjectResult(Constant.Exception_OrderNotExist);
            this._orderServiceMock.Setup(q => q.GetByOrderIDAsync(orderID)).ReturnsAsync(order);

            //Act
            var result = await this._controller.GetAsync(orderID);

            // Assert
            this._orderServiceMock.Verify(q => q.GetByOrderIDAsync(orderID),
                "error in calling the correct method");  // Verifies that OrderService.GetByIdAsync was called
            TestHelper.AreEqualEntities(expectedResult, result, "error in returning correct BadRequestObjectResult");
        }

        [Test]
        public async Task GetAsync_ReturnsOK()
        {
            // Arrange
            long orderID = 5;
            var order = this.Entity;
            var orderDetails = new OrderDetailModel().EntityList;
            var productTypes = new ProductTypeModel().EntityList;
            var orderItems = orderDetails.ToDictionary(q => productTypes.Single(x => x.ProductTypeID.Equals(q.ProductTypeID)).Name,
                q => q.Quantity);
            var orderInformation = new OrderInformation(orderItems, order.RequiredBinWidth);
            var expectedResult = new OkObjectResult(orderInformation);
            this._orderServiceMock.Setup(q => q.GetByOrderIDAsync(orderID)).ReturnsAsync(order);
            this._orderDetailServiceMock.Setup(q => q.GetListByOrderIDAsync(orderID)).ReturnsAsync(orderDetails);
            this._productTypeServiceMock.Setup(q => q.GetAllAsync()).ReturnsAsync(productTypes);

            //Act
            var result = await this._controller.GetAsync(orderID);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result, "error in returning correct result type");
            this._orderServiceMock.Verify(q => q.GetByOrderIDAsync(orderID),
                "error in calling the correct method");  // Verifies that OrderService.GetByIdAsync was called
            this._orderDetailServiceMock.Verify(q => q.GetListByOrderIDAsync(orderID),
                "error in calling the correct method");  // Verifies that OrderDetailService.GetListByOrderIDAsync was called
            this._productTypeServiceMock.Verify(q => q.GetAllAsync(),
                "error in calling the correct method");  // Verifies that ProductTypeService.GetAllAsync was called
            TestHelper.AreEqualEntities(expectedResult, result, "error in returning correct entity");
        }

        #endregion /GetAsync

        #region PostAsync

        [Test]
        public async Task PostAsync_NullCustomerOrder_ReturnsBadRequestInvalidOrder()
        {
            // Arrange
            CustomerOrder customerOrder = null;
            var expectedResult = new BadRequestObjectResult(Constant.Exception_InvalidOrder);

            //Act
            var result = await this._controller.PostAsync(customerOrder);

            // Assert
            TestHelper.AreEqualEntities(expectedResult, result, "error in returning correct BadRequestObjectResult");
        }

        /// <summary>
        /// Other exceptions are like below, so I don't write tests for them
        /// </summary>
        [Test]
        public async Task PostAsync_InvalidCustomerOrder_ReturnsBadRequest()
        {
            // Arrange
            CustomerOrder customerOrder = new OrderModel().CustomerOrder_WithInvalidOrderID;
            var transResult = new TransactionResult(new CustomException(Constant.Exception_InvalidOrderID));
            var expectedResult = new BadRequestObjectResult(Constant.Exception_InvalidOrderID);
            this._orderServiceMock.Setup(q => q.SaveOrderAsync(customerOrder.OrderID, customerOrder.OrderItems))
                .ReturnsAsync(transResult);

            //Act
            var result = await this._controller.PostAsync(customerOrder);

            // Assert
            this._orderServiceMock.Verify(q => q.SaveOrderAsync(customerOrder.OrderID, customerOrder.OrderItems),
                "error in calling the correct method");  // Verifies that OrderService.SaveOrderAsync was called
            TestHelper.AreEqualEntities(expectedResult, result, "error in returning correct BadRequestObjectResult");
        }

        [Test]
        public async Task PostAsync_RequiredBinWidthSimple_ReturnsOK()
        {
            // Arrange
            var model = new OrderModel();
            CustomerOrder customerOrder = model.CustomerOrder_Simple;
            float requiredBinWidth = model.RequiredBinWidth_Simple;
            var transResult = new TransactionResult(requiredBinWidth);
            var expectedResult = new ObjectResult(string.Format("{0} mm", requiredBinWidth)) { StatusCode = 201 };
            this._orderServiceMock.Setup(q => q.SaveOrderAsync(customerOrder.OrderID, customerOrder.OrderItems))
                .ReturnsAsync(transResult);

            //Act
            var result = await this._controller.PostAsync(customerOrder);

            // Assert            
            Assert.IsInstanceOf<ObjectResult>(result, "error in returning correct result type");
            this._orderServiceMock.Verify(q => q.SaveOrderAsync(customerOrder.OrderID, customerOrder.OrderItems),
                "error in calling the correct method");  // Verifies that OrderService.SaveOrderAsync was called
            TestHelper.AreEqualEntities(expectedResult, result, "error in returning correct ObjectResult");
        }

        [Test]
        public async Task PostAsync_RequiredBinWidthSample1_ReturnsOK()
        {
            // Arrange
            var model = new OrderModel();
            CustomerOrder customerOrder = model.CustomerOrder_Sample1;
            float requiredBinWidth = model.RequiredBinWidth_Sample1And2;
            var transResult = new TransactionResult(requiredBinWidth);
            var expectedResult = new ObjectResult(string.Format("{0} mm", requiredBinWidth)) { StatusCode = 201 };
            this._orderServiceMock.Setup(q => q.SaveOrderAsync(customerOrder.OrderID, customerOrder.OrderItems))
                .ReturnsAsync(transResult);

            //Act
            var result = await this._controller.PostAsync(customerOrder);

            // Assert            
            Assert.IsInstanceOf<ObjectResult>(result, "error in returning correct result type");
            this._orderServiceMock.Verify(q => q.SaveOrderAsync(customerOrder.OrderID, customerOrder.OrderItems),
                "error in calling the correct method");  // Verifies that OrderService.SaveOrderAsync was called
            TestHelper.AreEqualEntities(expectedResult, result, "error in returning correct ObjectResult");
        }

        [Test]
        public async Task PostAsync_RequiredBinWidthSample2_ReturnsOK()
        {
            // Arrange
            var model = new OrderModel();
            CustomerOrder customerOrder = model.CustomerOrder_Sample2;
            float requiredBinWidth = model.RequiredBinWidth_Sample1And2;
            var transResult = new TransactionResult(requiredBinWidth);
            var expectedResult = new ObjectResult(string.Format("{0} mm", requiredBinWidth)) { StatusCode = 201 };
            this._orderServiceMock.Setup(q => q.SaveOrderAsync(customerOrder.OrderID, customerOrder.OrderItems))
                .ReturnsAsync(transResult);

            //Act
            var result = await this._controller.PostAsync(customerOrder);

            // Assert            
            Assert.IsInstanceOf<ObjectResult>(result, "error in returning correct result type");
            this._orderServiceMock.Verify(q => q.SaveOrderAsync(customerOrder.OrderID, customerOrder.OrderItems),
                "error in calling the correct method");  // Verifies that OrderService.SaveOrderAsync was called
            TestHelper.AreEqualEntities(expectedResult, result, "error in returning correct ObjectResult");
        }

        [Test]
        public async Task PostAsync_RequiredBinWidthSample3_ReturnsOK()
        {
            // Arrange
            var model = new OrderModel();
            CustomerOrder customerOrder = model.CustomerOrder_Sample3;
            float requiredBinWidth = model.RequiredBinWidth_Sample3;
            var transResult = new TransactionResult(requiredBinWidth);
            var expectedResult = new ObjectResult(string.Format("{0} mm", requiredBinWidth)) { StatusCode = 201 };
            this._orderServiceMock.Setup(q => q.SaveOrderAsync(customerOrder.OrderID, customerOrder.OrderItems))
                .ReturnsAsync(transResult);

            //Act
            var result = await this._controller.PostAsync(customerOrder);

            // Assert            
            Assert.IsInstanceOf<ObjectResult>(result, "error in returning correct result type");
            this._orderServiceMock.Verify(q => q.SaveOrderAsync(customerOrder.OrderID, customerOrder.OrderItems),
                "error in calling the correct method");  // Verifies that OrderService.SaveOrderAsync was called
            TestHelper.AreEqualEntities(expectedResult, result, "error in returning correct ObjectResult");
        }

        [Test]
        public async Task PostAsync_RequiredBinWidthComplex_ReturnsOK()
        {
            // Arrange
            var model = new OrderModel();
            CustomerOrder customerOrder = model.CustomerOrder_Complex;
            float requiredBinWidth = model.RequiredBinWidth_Complex;
            var transResult = new TransactionResult(requiredBinWidth);
            var expectedResult = new ObjectResult(string.Format("{0} mm", requiredBinWidth)) { StatusCode = 201 };
            this._orderServiceMock.Setup(q => q.SaveOrderAsync(customerOrder.OrderID, customerOrder.OrderItems))
                .ReturnsAsync(transResult);

            //Act
            var result = await this._controller.PostAsync(customerOrder);

            // Assert            
            Assert.IsInstanceOf<ObjectResult>(result, "error in returning correct result type");
            this._orderServiceMock.Verify(q => q.SaveOrderAsync(customerOrder.OrderID, customerOrder.OrderItems),
                "error in calling the correct method");  // Verifies that OrderService.SaveOrderAsync was called
            TestHelper.AreEqualEntities(expectedResult, result, "error in returning correct ObjectResult");
        }

        [Test]
        public async Task PostAsync_RequiredBinWidthWithOneZeroQuantity_ReturnsOK()
        {
            // Arrange
            var model = new OrderModel();
            CustomerOrder customerOrder = model.CustomerOrder_WithOneZeroQuantity;
            float requiredBinWidth = model.RequiredBinWidth_WithOneZeroQuantity;
            var transResult = new TransactionResult(requiredBinWidth);
            var expectedResult = new ObjectResult(string.Format("{0} mm", requiredBinWidth)) { StatusCode = 201 };
            this._orderServiceMock.Setup(q => q.SaveOrderAsync(customerOrder.OrderID, customerOrder.OrderItems))
                .ReturnsAsync(transResult);

            //Act
            var result = await this._controller.PostAsync(customerOrder);

            // Assert            
            Assert.IsInstanceOf<ObjectResult>(result, "error in returning correct result type");
            this._orderServiceMock.Verify(q => q.SaveOrderAsync(customerOrder.OrderID, customerOrder.OrderItems),
                "error in calling the correct method");  // Verifies that OrderService.SaveOrderAsync was called
            TestHelper.AreEqualEntities(expectedResult, result, "error in returning correct ObjectResult");
        }

        #endregion /PostAsync

        #endregion /Methods

    }
}
