using Microsoft.Extensions.Configuration;
using Store.BuisnessLogic.Helpers;
using Store.BuisnessLogic.Helpers.Mappers.RequestFilterMappers;
using Store.BuisnessLogic.Helpers.Mappers.ResponseFilterMappers;
using Store.BuisnessLogic.Models.Filters.ResponseFilters;
using Store.BuisnessLogic.Models.Orders;
using Store.BuisnessLogicLayer.Models.Filters;
using Store.BuisnessLogicLayer.Models.Orders;
using Store.BuisnessLogicLayer.Models.Payments;
using Store.BuisnessLogicLayer.Models.PrintingEditions;
using Store.BuisnessLogicLayer.Models.Users;
using Store.BuisnessLogicLayer.Services.Interfaces;
using Store.DataAccess.Filters.ResponseFulters;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Store.BuisnessLogicLayer.Models.Enums.Enums;

namespace Store.BuisnessLogicLayer.Services
{
    public class OrderService: IOrderService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IConfiguration _configuration;
        private readonly IPrintingEditionRepository _printingEditionRepository;
        private readonly IUserRepository _userRepository;

        private readonly Mapper<User, UserModel> _userModelMapper = new Mapper<User, UserModel>();
        private readonly Mapper<Order, OrderModel> _orderModelMapper = new Mapper<Order, OrderModel>();
        private readonly Mapper<OrderItemModel, OrderItem> _orderItemMapper = 
            new Mapper<OrderItemModel, OrderItem>();
        private readonly Mapper<OrderItem, OrderItemModel> _orderItemModelMapper = new Mapper<OrderItem, OrderItemModel>();
        private readonly Mapper<PrintingEdition, PrintingEditionModel> _printingEditionModelMapper =
            new Mapper<PrintingEdition, PrintingEditionModel>();

        public OrderService(IPaymentRepository paymentRepository, 
            IOrderItemRepository orderItemRepository, IOrderRepository orderRepository,
            IConfiguration configuration, IPrintingEditionRepository printingEditionRepository,
            IUserRepository userRepository)
        {
            _paymentRepository = paymentRepository;
            _orderItemRepository = orderItemRepository;
            _orderRepository = orderRepository;
            _configuration = configuration;
            _printingEditionRepository = printingEditionRepository;
            _userRepository = userRepository;
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
            var orderResponse = _orderRepository.Filter(filter);
            var orderResponseModel = OrderResponseFilterMapper.Map(orderResponse);

            for (int i = 0; i < orderResponse.Orders.Count(); i++)
            {
                var orderItems = GetOrderItems(orderResponse.Orders[i].Id);
                var user = await _userRepository.GetAsync(orderResponse.Orders[i].UserId);
                var userModel = _userModelMapper.Map(new UserModel(), user);
                orderResponseModel.Orders[i].OrderItems = orderItems;
                orderResponseModel.Orders[i].User = userModel;
            }
            return orderResponseModel;
        }

        public async Task<List<OrderModel>> GetUserOrdersAsync(Guid userId)
        {
            var orders = await _orderRepository.GetUserOrdersAsync(userId);
            var orderModels = new List<OrderModel>();
            foreach (var order in orders)
            {
                var orderModel = _orderModelMapper.Map(new OrderModel(), order);
                orderModel.Status = (OrderStatus)order.Status;
                orderModel.OrderItems = GetOrderItems(order.Id);
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

        private List<OrderItemModel> GetOrderItems(Guid orderId)
        {
            var orderItems = _orderRepository.GetOrderItems(orderId);
            var orderItemModels = new List<OrderItemModel>();
            foreach (var orderItem in orderItems)
            {
                var orderItemModel = _orderItemModelMapper.Map(new OrderItemModel(), orderItem);
                var pe = _printingEditionRepository.GetAsync(orderItem.PrintingEditionId).Result;
                orderItemModel.PrintingEdition = _printingEditionModelMapper.Map(new PrintingEditionModel(), pe);
                orderItemModels.Add(orderItemModel);
            }
            return orderItemModels;
        }
    }
}
