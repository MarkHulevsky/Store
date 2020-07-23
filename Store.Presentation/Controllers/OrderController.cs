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
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _orderService.GetAllAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetFiltred([FromBody]OrderRequestFilterModel filter)
        {
            var ordersResponse = await _orderService.FilterAsync(filter);
            return Ok(ordersResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CartModel cartModel)
        {
            return Ok(await _orderService.CreateAsync(cartModel));
        }

        [HttpPost]
        public IActionResult Pay([FromBody]PaymentModel paymentModel)
        {
            _orderService.PayOrder(paymentModel);
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete]
        public async Task Remove([FromBody]string orderId)
        {
            await _orderService.RemoveAsync(Guid.Parse(orderId));
        }
    }
}
