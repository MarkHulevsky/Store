using Microsoft.Extensions.Configuration;
using Store.BuisnessLogic.Helpers;
using Store.BuisnessLogic.Models.Orders;
using Store.BuisnessLogicLayer.Models.Base;
using Store.BuisnessLogicLayer.Models.Filters;
using Store.BuisnessLogicLayer.Models.Orders;
using Store.BuisnessLogicLayer.Models.Payments;
using Store.BuisnessLogicLayer.Services.Interfaces;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Filters;
using Store.DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Store.DataAccessLayer.Entities.Enums.Enums;

namespace Store.BuisnessLogicLayer.Services
{
    public class OrderService: IOrderService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IConfiguration _configuration;

        private readonly Mapper<PagingModel, Paging> _pagingMapper = new Mapper<PagingModel, Paging>();
        private readonly Mapper<Order, OrderModel> _orderModelMapper = new Mapper<Order, OrderModel>();
        private readonly Mapper<OrderModel, Order> _orderMapper = new Mapper<OrderModel, Order>();
        private readonly Mapper<OrderItem, OrderItemModel> _orderItemModelMapper = new Mapper<OrderItem ,OrderItemModel>();
        private readonly Mapper<OrderItemModel, OrderItem> _orderItemMapper = new Mapper<OrderItemModel, OrderItem>();
        private readonly Mapper<OrderRequestFilterModel, OrderRequestFilter> _filterMapper =
            new Mapper<OrderRequestFilterModel, OrderRequestFilter>();

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
            var tokenService = new Stripe.TokenService();
            var cardOptions = new Stripe.TokenCreateOptions
            {
                Card = new Stripe.CreditCardOptions
                {
                    Number = paymentModel.Card.CardNumber,
                    ExpYear = paymentModel.Card.ExpYear,
                    ExpMonth = paymentModel.Card.ExpMonth,
                    Cvc = paymentModel.Card.CVC
                }
            };
            var token = tokenService.Create(cardOptions);

            var customerService = new Stripe.CustomerService();
            var chargeService = new Stripe.ChargeService();

            var customerOptions = new Stripe.CustomerCreateOptions
            {
                Email = paymentModel.UserEmail,
                Source = token.Id
            };

            var customer = customerService.Create(customerOptions);

            var chargeOptions = new Stripe.ChargeCreateOptions
            {
                Amount = paymentModel.Amount,
                Currency = paymentModel.Currency,
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

        public async Task<List<OrderModel>> FilterAsync(OrderRequestFilterModel filterModel)
        {
            var orderStatuses = new List<OrderStatus>();
            foreach (var statusModel in filterModel.OrderStatuses)
            {
                orderStatuses.Add((OrderStatus)statusModel);

            }
            var paging = _pagingMapper.Map(new Paging(), filterModel.Paging);

            var filter = _filterMapper.Map(new OrderRequestFilter(), filterModel);
            filter.Paging = paging;
            var orders = await _orderRepository.FilterAsync(filter);
            var orderModels = new List<OrderModel>();
            foreach (var order in orders)
            {
                var orderItemsModel = GetOrderItemModels(order.OrderItems);
                var orderModel = _orderModelMapper.Map(new OrderModel(), order);
                orderModel.OrderItems = orderItemsModel;
                orderModels.Add(orderModel);
            }
            return orderModels;
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

        public async Task<BaseModel> CreateAsync(CartModel cartModel)
        {
            var order = _orderMapper.Map(new Order(), cartModel.Order);
            order.UserId = cartModel.UserId;
            var orderItems = new List<OrderItem>();
            order = await _orderRepository.CreateAsync(order);
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

        private List<OrderItemModel> GetOrderItemModels(IEnumerable<OrderItem> orderItems)
        {
            var orderItemModels = new List<OrderItemModel>();
            foreach (var orderItem in orderItems)
            {
                var orderItemModel = _orderItemModelMapper.Map(new OrderItemModel(), orderItem);
                orderItemModels.Add(orderItemModel);
            }
            return orderItemModels;
        }
    }
}
