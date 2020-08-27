using Microsoft.Extensions.Configuration;
using Store.BuisnessLogic.Helpers;
using Store.BuisnessLogic.Helpers.Mappers.ListMappers;
using Store.BuisnessLogic.Helpers.Mappers.RequestFilterMappers;
using Store.BuisnessLogic.Helpers.Mappers.ResponseFilterMappers;
using Store.BuisnessLogic.Models.Filters;
using Store.BuisnessLogic.Models.Filters.ResponseFilters;
using Store.BuisnessLogic.Models.Orders;
using Store.BuisnessLogic.Models.Payments;
using Store.BuisnessLogic.Services.Interfaces;
using Store.DataAccess.Entities;
using Store.DataAccess.Repositories.Interfaces;
using Stripe;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Order = Store.DataAccess.Entities.Order;
using OrderItem = Store.DataAccess.Entities.OrderItem;

namespace Store.BuisnessLogic.Services
{
    public class OrderService : IOrderService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IConfiguration _configuration;

        private readonly Mapper<Order, OrderModel> _orderModelMapper;
        private readonly Mapper<OrderItemModel, OrderItem> _orderItemMapper;

        private const string chargeSucceeded = "succeeded";

        public OrderService(IPaymentRepository paymentRepository,
            IOrderItemRepository orderItemRepository, IOrderRepository orderRepository,
            IConfiguration configuration)
        {
            _paymentRepository = paymentRepository;
            _orderItemRepository = orderItemRepository;
            _orderRepository = orderRepository;
            _configuration = configuration;
            _orderItemMapper = new Mapper<OrderItemModel, OrderItem>();
            _orderModelMapper = new Mapper<Order, OrderModel>();
        }

        public void PayOrder(PaymentModel paymentModel)
        {
            StripeConfiguration.ApiKey = _configuration.GetSection("Stripe")["SecretKey"];
            var customerService = new CustomerService();
            var chargeService = new ChargeService();

            var customerOptions = new CustomerCreateOptions
            {
                Email = paymentModel.UserEmail,
                Source = paymentModel.TokenId
            };

            var customer = customerService.Create(customerOptions);

            var chargeOptions = new ChargeCreateOptions
            {
                Amount = paymentModel.Amount,
                Currency = paymentModel.CurrencyString,
                Customer = customer.Id
            };

            var charge = chargeService.Create(chargeOptions);

            if (charge.Status == chargeSucceeded)
            {
                var payment = new Payment()
                {
                    TransactionId = charge.BalanceTransactionId
                };
                payment = _paymentRepository.CreateAsync(payment).Result;
                _orderRepository.AddToPaymentAsync(payment.Id, paymentModel.OrderId).Wait();
            }
        }

        public async Task<OrderResponseModel> FilterAsync(OrderRequestModel filterModel)
        {
            var filter = OrderRequestMapper.Map(filterModel);
            var orderResponse = await _orderRepository.FilterAsync(filter);
            var orderResponseModel = OrderResponseFilterMapper.Map(orderResponse);
            return orderResponseModel;
        }

        public async Task<List<OrderModel>> GetUserOrdersAsync(Guid userId)
        {
            var orders = await _orderRepository.GetUserOrdersAsync(userId);
            var orderModels = OrderModelListMapper.Map(orders);
            return orderModels;
        }

        public async Task<OrderModel> CreateAsync(CartModel cartModel)
        {
            var order = new Order
            {
                UserId = cartModel.UserId
            };
            order = await _orderRepository.CreateAsync(order);
            var orderModel = _orderModelMapper.Map(order);
            foreach (var orderItemModel in cartModel.Order.OrderItems)
            {
                var orderItem = _orderItemMapper.Map(orderItemModel);
                orderItem.OrderId = order.Id;
                await _orderItemRepository.CreateAsync(orderItem);
            }
            return orderModel;
        }

        public async Task RemoveAsync(Guid id)
        {
            await _orderRepository.RemoveAsync(id);
        }
    }
}
