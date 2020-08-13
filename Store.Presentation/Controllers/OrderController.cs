using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.BuisnessLogic.Models.Orders;
using Store.BuisnessLogicLayer.Models.Filters;
using Store.BuisnessLogicLayer.Models.Payments;
using Store.BuisnessLogicLayer.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Store.Presentation.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        public OrderController(IOrderService orderService, IUserService userService)
        {
            _orderService = orderService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserOrders()
        {
            var currentUser = await _userService.GetCurrentAsync(HttpContext.User);
            var orders = await _orderService.GetUserOrdersAsync(currentUser.Id);
            return Ok(orders);
        }

        [HttpPost]
        public async Task<IActionResult> GetFiltred([FromBody] OrderRequestFilterModel filter)
        {
            var ordersResponse = await _orderService.FilterAsync(filter);
            return Ok(ordersResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CartModel cartModel)
        {
            var currentUser = await _userService.GetCurrentAsync(HttpContext.User);
            cartModel.UserId = currentUser.Id;
            var orderModel = await _orderService.CreateAsync(cartModel);
            return Ok(orderModel);
        }

        [HttpPost]
        public IActionResult Pay([FromBody] PaymentModel paymentModel)
        {
            _orderService.PayOrder(paymentModel);
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete]
        public async Task Remove([FromBody] string orderId)
        {
            await _orderService.RemoveAsync(Guid.Parse(orderId));
        }
    }
}
