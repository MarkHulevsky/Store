using Microsoft.Extensions.Configuration;
using Store.BuisnessLogic.Helpers;
using Store.BuisnessLogic.Helpers.Mappers.RequestFilterMappers;
using Store.BuisnessLogic.Helpers.Mappers.ResponseFilterMappers;
using Store.BuisnessLogic.Models.Filters.ResponseFilters;
using Store.BuisnessLogic.Models.Orders;
using Store.BuisnessLogicLayer.Models.Filters;
using Store.BuisnessLogicLayer.Models.Orders;
using Store.BuisnessLogicLayer.Models.Payments;
using Store.BuisnessLogicLayer.Services.Interfaces;
using Store.DataAccess.Filters.ResponseFulters;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.BuisnessLogicLayer.Services
{
    public class OrderService: IOrderService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IConfiguration _configuration;

        private readonly Mapper<Order, OrderModel> _orderModelMapper = new Mapper<Order, OrderModel>();
        private readonly Mapper<OrderModel, Order> _orderMapper = new Mapper<OrderModel, Order>();
        private readonly Mapper<OrderResponseFilter, OrderResponseFilterModel> _responseMapper = 
            new Mapper<OrderResponseFilter, OrderResponseFilterModel>();
        private readonly Mapper<OrderItemModel, OrderItem> _orderItemMapper = 
            new Mapper<OrderItemModel, OrderItem>();

        public OrderService(IPaymentRepository paymentRepository, 
            IOrderItemRepository orderItemRepository, IOrderRepository orderRepository,
            IConfiguration configuration)
        {
            _paymentRepository = paymentRepository;
            _orderItemRepository = orderItemRepository;
            _orderRepository = orderRepository;
            _configuration = configuration;
        }

        public void PayOrder(PaymentModel paymentModel)
        {
            Stripe.StripeConfiguration.ApiKey = _configuration.GetSection("Stripe")["SecretKey"];
            var customerService = new Stripe.CustomerService();
            var chargeService = new Stripe.ChargeService();

            var customerOptions = new Stripe.CustomerCreateOptions
            {
                Email = paymentModel.UserEmail,
                Source = paymentModel.TokenId
            };

            var customer = customerService.Create(customerOptions);

            var chargeOptions = new Stripe.ChargeCreateOptions
            {
                Amount = paymentModel.Amount,
                Currency = paymentModel.CurrencyString,
                Customer = customer.Id
            };

            var charge = chargeService.Create(chargeOptions);
            
            if (charge.Status == "succeeded")
            {
                var payment = new Payment()
                {
                    TransactionId = charge.BalanceTransactionId
                };
                payment = _paymentRepository.CreateAsync(payment).Result;
                _orderRepository.AddToPaymentAsync(payment.Id, paymentModel.OrderId).Wait();
            }
        }

        public async Task<OrderResponseFilterModel> FilterAsync(OrderRequestFilterModel filterModel)
        {
            var filter = OrderRequestFilterMapper.Map(filterModel);
            var orderResponse = await _orderRepository.FilterAsync(filter);
            var orderResponseModel = OrderResponseFilterMapper.Map(orderResponse);
            return orderResponseModel;
        }

        public async Task<List<OrderModel>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            var orderModels = new List<OrderModel>();
            
            foreach (var order in orders)
            {
                var orderModel = _orderModelMapper.Map(new OrderModel(), order);
                orderModels.Add(orderModel);
            }
            return orderModels;
        }

        public async Task<List<OrderModel>> GetUserOrdersAsync(Guid userId)
        {
            var orders = await _orderRepository.GetUserOrdersAsync(userId);
            var orderModels = new List<OrderModel>();
            foreach (var order in orders)
            {
                var orderModel = _orderModelMapper.Map(new OrderModel(), order);
                orderModels.Add(orderModel);
            }
            return orderModels;
        }

        public async Task<OrderModel> CreateAsync(CartModel cartModel)
        {
            var order = new Order();
            order.UserId = cartModel.UserId;
            var orderItems = new List<OrderItem>();
            order = await _orderRepository.CreateAsync(order);
            cartModel.Order.Id = order.Id;
            foreach (var orderItemModel in cartModel.Order.OrderItems)
            {
                var orderItem = _orderItemMapper.Map(new OrderItem(), orderItemModel);
                orderItem.OrderId = order.Id;
                await _orderItemRepository.CreateAsync(orderItem);
                orderItems.Add(orderItem);
            }
            return cartModel.Order;
        }

        public async Task RemoveAsync(Guid id)
        {
            await _orderRepository.RemoveAsync(id);
        }
    }
}
