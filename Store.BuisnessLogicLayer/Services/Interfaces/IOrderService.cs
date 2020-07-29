using Store.BuisnessLogic.Models.Filters.ResponseFilters;
using Store.BuisnessLogic.Models.Orders;
using Store.BuisnessLogicLayer.Models.Base;
using Store.BuisnessLogicLayer.Models.Filters;
using Store.BuisnessLogicLayer.Models.Orders;
using Store.BuisnessLogicLayer.Models.Payments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.BuisnessLogicLayer.Services.Interfaces
{
    public interface IOrderService
    {
        void PayOrder(PaymentModel paymentModel);
        Task<List<OrderModel>> GetAllAsync();
        Task<List<OrderModel>> GetUserOrdersAsync(Guid userId);
        Task<OrderModel> CreateAsync(CartModel cartModel);
        Task RemoveAsync(Guid id);
        Task<OrderResponseFilterModel> FilterAsync(OrderRequestFilterModel filter);
    }
}
