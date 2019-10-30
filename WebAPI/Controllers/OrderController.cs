using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Core.ApplicationService.Contracts;
using Core.DomainModel;
using Core.DomainService;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        #region Properties

        private readonly IOrderService _orderService;

        private readonly IOrderDetailService _orderDetailService;

        private readonly IProductTypeService _productTypeService;

        #endregion /Properties

        #region Constructors

        public OrderController(IOrderService orderService,
            IOrderDetailService orderDetailService,
            IProductTypeService productTypeService)
        {
            this._orderService = orderService;
            this._orderDetailService = orderDetailService;
            this._productTypeService = productTypeService;
        }

        #endregion /Constructors

        #region Methods

        // GET: api/order/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> GetAsync(long id)
        {
            var order = await this._orderService.GetByOrderIDAsync(id);
            if (order == null)
            {
                return BadRequest(Constant.Exception_OrderNotExist);
            }
            var orderDetails = this._orderDetailService.GetListByOrderID(id);
            var productTypes = this._productTypeService.GetAll();
            var orderItems = orderDetails.ToDictionary(q => productTypes.Single(x => x.ProductTypeID.Equals(q.ProductTypeID)).Name, q => q.Quantity);
            var orderInformation = new OrderInformation(orderItems, order.RequiredBinWidth);
            return new OkObjectResult(orderInformation);
        }

        // POST: api/Bank
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]CustomerOrder customerOrder)
        {
            TransactionResult result;
            if (customerOrder == null)
            {
                return BadRequest(Constant.Exception_InvalidOrder);
            }
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled)) // not need in this case,
                                                                                              // I already managed in unit of work
                                                                                              // but I always use it for guaranting transactional operations
            {
                result = await this._orderService.SaveOrderAsync(customerOrder.OrderID, customerOrder.OrderItems);
                if (result.IsSuccessful)
                {
                    scope.Complete();
                    return StatusCode(201, string.Format(Constant.RequiredBinWidth, result.Content)); // Created
                }
            }
            return BadRequest(result.ExceptionContentResult);
        }

        #endregion /Methods

    }
}
