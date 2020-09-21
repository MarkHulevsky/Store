using Store.BuisnessLogic.Helpers;
using Store.BuisnessLogic.Helpers.Mappers.ListMappers;
using Store.BuisnessLogic.Helpers.Mappers.RequestFilterMappers;
using Store.BuisnessLogic.Helpers.Mappers.ResponseFilterMappers;
using Store.BuisnessLogic.Models.Base;
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

namespace Store.BuisnessLogic.Services
{
    public class OrderService : IOrderService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly Mapper<Order, OrderModel> _orderModelMapper;

        private const string CHARGE_SUCCEEDED = "succeeded";
        private const string CHARGE_IS_NOT_SUCCEEDED_ERROR = "Charge isn't succeeded";

        public OrderService(IPaymentRepository paymentRepository,
            IOrderItemRepository orderItemRepository, IOrderRepository orderRepository)
        {
            _paymentRepository = paymentRepository;
            _orderItemRepository = orderItemRepository;
            _orderRepository = orderRepository;
            _orderModelMapper = new Mapper<Order, OrderModel>();
        }

        public async Task<BaseModel> PayOrderAsync(PaymentModel paymentModel)
        {
            var customerService = new CustomerService();
            var chargeService = new ChargeService();

            var customerOptions = new CustomerCreateOptions
            {
                Email = paymentModel.UserEmail,
                Source = paymentModel.TokenId
            };

            var customer = await customerService.CreateAsync(customerOptions);

            var chargeOptions = new ChargeCreateOptions
            {
                Amount = paymentModel.Amount,
                Currency = paymentModel.CurrencyString,
                Customer = customer.Id
            };

            var charge = await chargeService.CreateAsync(chargeOptions);

            if (charge.Status != CHARGE_SUCCEEDED)
            {
                return new BaseModel
                {
                    Errors = new List<string>
                    {
                        CHARGE_IS_NOT_SUCCEEDED_ERROR
                    }
                };
            }
            var payment = new Payment()
            {
                TransactionId = charge.BalanceTransactionId
            };
            payment = await _paymentRepository.CreateAsync(payment);
            await _orderRepository.AddToPaymentAsync(payment.Id, paymentModel.OrderId);
            return new BaseModel();
        }

        public async Task<OrderResponseModel> FilterAsync(OrderRequestModel orderRequestModel)
        {
            var orderRequestDataModel = OrderRequestMapper.Map(orderRequestModel);
            var orderResponseDataModel = await _orderRepository.FilterAsync(orderRequestDataModel);
            var orderResponseModel = OrderResponseMapper.Map(orderResponseDataModel);
            return orderResponseModel;
        }

        public async Task<List<OrderModel>> GetUserOrdersAsync(Guid userId)
        {
            var orders = await _orderRepository.GetUserOrdersAsync(userId);
            var orderModels = OrderModelListMapper.Map(orders);
            return orderModels;
        }

        public async Task<OrderModel> CreateAsync(CartModel cartModel, Guid userId)
        {
            var order = new Order
            {
                UserId = userId
            };
            order = await _orderRepository.CreateAsync(order);
            var orderModel = _orderModelMapper.Map(order);
            var orderItems = OrderItemListMapper.Map(cartModel.Order.OrderItems, order.Id);
            await _orderItemRepository.AddRangeAsync(orderItems);
            return orderModel;
        }

        public async Task RemoveAsync(string orderId)
        {
            var id = Guid.Parse(orderId);
            await _orderRepository.RemoveAsync(id);
        }
    }
}
