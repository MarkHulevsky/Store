﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.BuisnessLogic.Models.Filters;
using Store.BuisnessLogic.Models.Orders;
using Store.BuisnessLogic.Models.Payments;
using Store.BuisnessLogic.Services.Interfaces;
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
            var currentUser = await _userService.GetCurrentAsync();
            var orders = await _orderService.GetUserOrdersAsync(currentUser.Id);
            return Ok(orders);
        }

        [HttpPost]
        public async Task<IActionResult> GetFiltred([FromBody] OrderRequestModel orderRequestModel)
        {
            var ordersResponse = await _orderService.FilterAsync(orderRequestModel);
            return Ok(ordersResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CartModel cartModel)
        {
            var userModel = await _userService.GetCurrentAsync();
            var orderModel = await _orderService.CreateAsync(cartModel, userModel.Id);
            return Ok(orderModel);
        }

        [HttpPost]
        public async Task<IActionResult> Pay([FromBody] PaymentModel paymentModel)
        {
            await _orderService.PayOrderAsync(paymentModel);
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete]
        public async Task Remove([FromBody] string orderId)
        {
            await _orderService.RemoveAsync(orderId);
        }
    }
}
