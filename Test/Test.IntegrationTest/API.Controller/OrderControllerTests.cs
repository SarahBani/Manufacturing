using Core.ApplicationService.Contracts;
using Core.DomainModel;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Test.Common;
using Test.Common.Models;
using WebAPI;

namespace Test.IntegrationTest.API.Controller
{
    [TestFixture]
    public class OrderControllerTests : BaseControllerTests
    {

        #region Properties

        private readonly string _getUrl = "/api/order/{0}";

        private readonly string _postUrl = "/api/order";

        private readonly string _requiredBinWidthText = string.Format(Constant.RequiredBinWidth, string.Empty);

        #endregion /Properties

        #region Constructors

        public OrderControllerTests()
            : base()
        {
        }

        #endregion /Constructors

        #region Methods

        [OneTimeSetUp]
        public override void Setup()
        {
        }

        #region GetAsync

        [Test]
        public async Task GetAsync_InvalidOrderID_ReturnsBadRequestOrderNotExist()
        {
            // Arrange
            long id = -1;
            string expectedContent = Constant.Exception_OrderNotExist;
            string url = string.Format(this._getUrl, id);

            //Act
            var result = await base.Client.GetAsync(url);

            // Assert
            var content = await base.GetDeserializedContent<string>(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest, "error in returning correct response");
            Assert.AreEqual(expectedContent, content, "error in returning correct content type");
        }

        [Test]
        public async Task GetAsync_ReturnsOK()
        {
            // Arrange
            long id = await GetMaxOrderID();
            if (id == 0)
            { /// The exception would be invalid OrderID which I check in another test method
                Assert.IsTrue(true, "error");
                return;
            }
            string url = string.Format(this._getUrl, id);

            //Act
            var result = await base.Client.GetAsync(url);

            // Assert
            var content = await base.GetDeserializedContent<OrderInformation>(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK, "error in returning correct response");
            Assert.IsInstanceOf<OrderInformation>(content, "error in returning correct content type");
        }

        #endregion /GetAsync

        #region PostAsync

        [Test]
        public async Task PostAsync_NullCustomerOrder_ReturnsBadRequestInvalidOrder()
        {
            // Arrange
            CustomerOrder order = null;
            var requestContent = base.GetSerializedContent(order);
            string expectedContent = Constant.Exception_InvalidOrder;
            string url = this._postUrl;

            //Act
            var result = await base.Client.PostAsync(url, requestContent);

            // Assert
            var responseContent = await base.GetDeserializedContent<string>(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest, "error in returning correct response");
            Assert.AreEqual(expectedContent, responseContent, "error in returning correct content type");
        }

        [Test]
        public async Task PostAsync_InvalidOrderID_ReturnsBadRequest()
        {
            // Arrange
            var order = new OrderModel().CustomerOrder_WithInvalidOrderID;
            var requestContent = base.GetSerializedContent(order);
            string expectedContent = Constant.Exception_InvalidOrderID;
            string url = this._postUrl;

            //Act
            var result = await base.Client.PostAsync(url, requestContent);

            // Assert
            var responseContent = await base.GetDeserializedContent<string>(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest, "error in returning correct response");
            Assert.AreEqual(expectedContent, responseContent, "error in returning correct content type");
        }

        [Test]
        public async Task PostAsync_DuplicateOrderID_ReturnsBadRequest()
        {
            // Arrange
            var order = new OrderModel().CustomerOrder_Simple;
            order.OrderID = await GetMaxOrderID();
            if (order.OrderID == 0)
            { /// The exception would be invalid OrderID which I check in another test method
                Assert.IsTrue(true, "error");
                return;
            }
            var requestContent = base.GetSerializedContent(order);
            string expectedContent = Constant.Exception_sql_KeyAlreadyExsits;
            string url = this._postUrl;

            //Act
            var result = await base.Client.PostAsync(url, requestContent);

            // Assert
            var responseContent = await base.GetDeserializedContent<string>(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest, "error in returning correct response");
            Assert.AreEqual(expectedContent, responseContent, "error in returning correct content type");
        }

        [Test]
        public async Task PostAsync_NullOrderItems_ReturnsBadRequest()
        {
            // Arrange
            var order = new OrderModel().CustomerOrder_WithNullOrderItems;
            var requestContent = base.GetSerializedContent(order);
            string expectedContent = Constant.Exception_OrderItemsNotExist;
            string url = this._postUrl;

            //Act
            var result = await base.Client.PostAsync(url, requestContent);

            // Assert
            var responseContent = await base.GetDeserializedContent<string>(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest, "error in returning correct response");
            Assert.AreEqual(expectedContent, responseContent, "error in returning correct content type");
        }

        [Test]
        public async Task PostAsync_OrderItemsWithoutAnyQuantity_ReturnsBadRequest()
        {
            // Arrange
            var order = new OrderModel().CustomerOrder_WithoutAnyQuantity;
            var requestContent = base.GetSerializedContent(order);
            string expectedContent = Constant.Exception_OrderItemsNotExist;
            string url = this._postUrl;

            //Act
            var result = await base.Client.PostAsync(url, requestContent);

            // Assert
            var responseContent = await base.GetDeserializedContent<string>(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest, "error in returning correct response");
            Assert.AreEqual(expectedContent, responseContent, "error in returning correct content type");
        }

        [Test]
        public async Task PostAsync_OrderItemsWithInvalidQuantity_ReturnsBadRequest()
        {
            // Arrange
            var order = new OrderModel().CustomerOrder_WithInvalidQuantity;
            var requestContent = base.GetSerializedContent(order);
            string expectedContent = Constant.Exception_InvalidOrderItemsQuantity;
            string url = this._postUrl;

            //Act
            var result = await base.Client.PostAsync(url, requestContent);

            // Assert
            var responseContent = await base.GetDeserializedContent<string>(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest, "error in returning correct response");
            Assert.AreEqual(expectedContent, responseContent, "error in returning correct content type");
        }

        [Test]
        public async Task PostAsync_OrderItemsWithInvalidProductType_ReturnsBadRequest()
        {
            // Arrange
            var order = new OrderModel().CustomerOrder_WithInvalidProductType;
            var requestContent = base.GetSerializedContent(order);
            string expectedContent = Constant.Exception_InvalidOrderItemsProductType;
            string url = this._postUrl;

            //Act
            var result = await base.Client.PostAsync(url, requestContent);

            // Assert
            var responseContent = await base.GetDeserializedContent<string>(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest, "error in returning correct response");
            Assert.AreEqual(expectedContent, responseContent, "error in returning correct content type");
        }

        [Test]
        public async Task PostAsync_CustomerOrderSimple_ReturnsOK()
        {
            // Arrange
            var model = new OrderModel();
            var order = model.CustomerOrder_Simple;
            order.OrderID = (await GetMaxOrderID()) + 1;
            var requestContent = base.GetSerializedContent(order);
            float expectedRequiredBinWidth = model.RequiredBinWidth_Simple;
            string url = this._postUrl;

            //Act
            var result = await base.Client.PostAsync(url, requestContent);

            // Assert
            float responseRequiredBinWidth = await GetResponseRequiredBinWidth(result);
            Assert.IsInstanceOf<float>(responseRequiredBinWidth, "error in returning correct content type");
            Assert.AreEqual(result.StatusCode, HttpStatusCode.Created, "error in returning correct response");
            Assert.AreEqual(expectedRequiredBinWidth, responseRequiredBinWidth, "error in returning correct RequiredBinWidth");
        }

        [Test]
        public async Task PostAsync_CustomerOrderSample1_ReturnsOK()
        {
            // Arrange
            var model = new OrderModel();
            var order = model.CustomerOrder_Sample1;
            order.OrderID = (await GetMaxOrderID()) + 1;
            var requestContent = base.GetSerializedContent(order);
            float expectedRequiredBinWidth = model.RequiredBinWidth_Sample1And2;
            string url = this._postUrl;

            //Act
            var result = await base.Client.PostAsync(url, requestContent);

            // Assert
            float responseRequiredBinWidth = await GetResponseRequiredBinWidth(result);
            Assert.IsInstanceOf<float>(responseRequiredBinWidth, "error in returning correct content type");
            Assert.AreEqual(result.StatusCode, HttpStatusCode.Created, "error in returning correct response");
            Assert.AreEqual(expectedRequiredBinWidth, responseRequiredBinWidth, "error in returning correct RequiredBinWidth");
        }

        [Test]
        public async Task PostAsync_CustomerOrderSample2_ReturnsOK()
        {
            // Arrange
            var model = new OrderModel();
            var order = model.CustomerOrder_Sample2;
            order.OrderID = (await GetMaxOrderID()) + 1;
            var requestContent = base.GetSerializedContent(order);
            float expectedRequiredBinWidth = model.RequiredBinWidth_Sample1And2;
            string url = this._postUrl;

            //Act
            var result = await base.Client.PostAsync(url, requestContent);

            // Assert
            float responseRequiredBinWidth = await GetResponseRequiredBinWidth(result);
            Assert.IsInstanceOf<float>(responseRequiredBinWidth, "error in returning correct content type");
            Assert.AreEqual(result.StatusCode, HttpStatusCode.Created, "error in returning correct response");
            Assert.AreEqual(expectedRequiredBinWidth, responseRequiredBinWidth, "error in returning correct RequiredBinWidth");
        }

        [Test]
        public async Task PostAsync_CustomerOrderSample3_ReturnsOK()
        {
            // Arrange
            var model = new OrderModel();
            var order = model.CustomerOrder_Sample3;
            order.OrderID = (await GetMaxOrderID()) + 1;
            var requestContent = base.GetSerializedContent(order);
            float expectedRequiredBinWidth = model.RequiredBinWidth_Sample3;
            string url = this._postUrl;

            //Act
            var result = await base.Client.PostAsync(url, requestContent);

            // Assert
            float responseRequiredBinWidth = await GetResponseRequiredBinWidth(result);
            Assert.IsInstanceOf<float>(responseRequiredBinWidth, "error in returning correct content type");
            Assert.AreEqual(result.StatusCode, HttpStatusCode.Created, "error in returning correct response");
            Assert.AreEqual(expectedRequiredBinWidth, responseRequiredBinWidth, "error in returning correct RequiredBinWidth");
        }

        [Test]
        public async Task PostAsync_CustomerOrderComplex_ReturnsOK()
        {
            // Arrange
            var model = new OrderModel();
            var order = model.CustomerOrder_Complex;
            order.OrderID = (await GetMaxOrderID()) + 1;
            var requestContent = base.GetSerializedContent(order);
            float expectedRequiredBinWidth = model.RequiredBinWidth_Complex;
            string url = this._postUrl;

            //Act
            var result = await base.Client.PostAsync(url, requestContent);

            // Assert
            float responseRequiredBinWidth = await GetResponseRequiredBinWidth(result);
            Assert.IsInstanceOf<float>(responseRequiredBinWidth, "error in returning correct content type");
            Assert.AreEqual(result.StatusCode, HttpStatusCode.Created, "error in returning correct response");
            Assert.AreEqual(expectedRequiredBinWidth, responseRequiredBinWidth, "error in returning correct RequiredBinWidth");
        }

        [Test]
        public async Task PostAsync_CustomerOrderWithOneZeroQuantity_ReturnsOK()
        {
            // Arrange
            var model = new OrderModel();
            var order = model.CustomerOrder_WithOneZeroQuantity;
            order.OrderID = (await GetMaxOrderID()) + 1;
            var requestContent = base.GetSerializedContent(order);
            float expectedRequiredBinWidth = model.RequiredBinWidth_WithOneZeroQuantity;
            string url = this._postUrl;

            //Act
            var result = await base.Client.PostAsync(url, requestContent);

            // Assert
            float responseRequiredBinWidth = await GetResponseRequiredBinWidth(result);
            Assert.IsInstanceOf<float>(responseRequiredBinWidth, "error in returning correct content type");
            Assert.AreEqual(result.StatusCode, HttpStatusCode.Created, "error in returning correct response");
            Assert.AreEqual(expectedRequiredBinWidth, responseRequiredBinWidth, "error in returning correct RequiredBinWidth");
        }

        #endregion /PostAsync

        #region PostAsync_GetAsync

        [Test]
        public async Task PostAsync_GetAsync_CustomerOrderSimple_ReturnsOK()
        {
            // Arrange
            var model = new OrderModel();
            var order = model.CustomerOrder_Simple;
            order.OrderID = (await GetMaxOrderID()) + 1;
            var requestContent = base.GetSerializedContent(order);
            var expectedResult = new OrderInformation(model.OrderItems_Simple, model.RequiredBinWidth_Simple);

            //Act
            await base.Client.PostAsync(this._postUrl, requestContent);
            var result = await base.Client.GetAsync(string.Format(this._getUrl, order.OrderID));

            // Assert
            var content = await base.GetDeserializedContent<OrderInformation>(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK, "error in returning correct response");
            TestHelper.AreEqualEntities(expectedResult, content, "error in returning correct content type");
        }

        [Test]
        public async Task PostAsync_GetAsync_CustomerOrderSample1_ReturnsOK()
        {
            // Arrange
            var model = new OrderModel();
            var order = model.CustomerOrder_Sample1;
            order.OrderID = (await GetMaxOrderID()) + 1;
            var requestContent = base.GetSerializedContent(order);
            var expectedResult = new OrderInformation(model.OrderItems_Sample1, model.RequiredBinWidth_Sample1And2);

            //Act
            await base.Client.PostAsync(this._postUrl, requestContent);
            var result = await base.Client.GetAsync(string.Format(this._getUrl, order.OrderID));

            // Assert
            var content = await base.GetDeserializedContent<OrderInformation>(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK, "error in returning correct response");
            TestHelper.AreEqualEntities(expectedResult, content, "error in returning correct content type");
        }

        [Test]
        public async Task PostAsync_GetAsync_CustomerOrderSample2_ReturnsOK()
        {
            // Arrange
            var model = new OrderModel();
            var order = model.CustomerOrder_Sample1;
            order.OrderID = (await GetMaxOrderID()) + 1;
            var requestContent = base.GetSerializedContent(order);
            var expectedResult = new OrderInformation(model.OrderItems_Sample2, model.RequiredBinWidth_Sample1And2);

            //Act
            await base.Client.PostAsync(this._postUrl, requestContent);
            var result = await base.Client.GetAsync(string.Format(this._getUrl, order.OrderID));

            // Assert
            var content = await base.GetDeserializedContent<OrderInformation>(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK, "error in returning correct response");
            TestHelper.AreEqualEntities(expectedResult, content, "error in returning correct content type");
        }

        [Test]
        public async Task PostAsync_GetAsync_CustomerOrderSample3_ReturnsOK()
        {
            // Arrange
            var model = new OrderModel();
            var order = model.CustomerOrder_Sample1;
            order.OrderID = (await GetMaxOrderID()) + 1;
            var requestContent = base.GetSerializedContent(order);
            var expectedResult = new OrderInformation(model.OrderItems_Sample3, model.RequiredBinWidth_Sample3);

            //Act
            await base.Client.PostAsync(this._postUrl, requestContent);
            var result = await base.Client.GetAsync(string.Format(this._getUrl, order.OrderID));

            // Assert
            var content = await base.GetDeserializedContent<OrderInformation>(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK, "error in returning correct response");
            TestHelper.AreEqualEntities(expectedResult, content, "error in returning correct content type");
        }

        [Test]
        public async Task PostAsync_GetAsync_CustomerOrderComplex_ReturnsOK()
        {
            // Arrange
            var model = new OrderModel();
            var order = model.CustomerOrder_Sample1;
            order.OrderID = (await GetMaxOrderID()) + 1;
            var requestContent = base.GetSerializedContent(order);
            var expectedResult = new OrderInformation(model.OrderItems_Complex, model.RequiredBinWidth_Complex);

            //Act
            await base.Client.PostAsync(this._postUrl, requestContent);
            var result = await base.Client.GetAsync(string.Format(this._getUrl, order.OrderID));

            // Assert
            var content = await base.GetDeserializedContent<OrderInformation>(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK, "error in returning correct response");
            TestHelper.AreEqualEntities(expectedResult, content, "error in returning correct content type");
        }

        #endregion /PostAsync_GetAsync

        private async Task<long> GetMaxOrderID()
        {
            using (var scope = base.Factory.Server.Host.Services.CreateScope())
            {
                var orderService = scope.ServiceProvider.GetService<IOrderService>();
                return await orderService.GetMaxOrderIDAsync();
            }
        }

        private async Task<float> GetResponseRequiredBinWidth(HttpResponseMessage response)
        {
            var responseContent = await base.GetDeserializedContent<string>(response);
            return float.Parse(responseContent.Replace(this._requiredBinWidthText, string.Empty));
        }

        #endregion /Methods

    }
}
