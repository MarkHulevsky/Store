using Store.BuisnessLogic.Models.Filters;
using Store.BuisnessLogic.Models.Filters.ResponseFilters;
using Store.BuisnessLogic.Models.Orders;
using Store.BuisnessLogic.Models.Payments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.BuisnessLogic.Services.Interfaces
{
    public interface IOrderService
    {
        Task PayOrderAsync(PaymentModel paymentModel);
        Task<List<OrderModel>> GetUserOrdersAsync(Guid userId);
        Task<OrderModel> CreateAsync(CartModel cartModel, Guid userId);
        Task RemoveAsync(string id);
        Task<OrderResponseModel> FilterAsync(OrderRequestModel orderRequestModel);
    }
}
