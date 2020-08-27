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
        void PayOrder(PaymentModel paymentModel);
        Task<List<OrderModel>> GetUserOrdersAsync(Guid userId);
        Task<OrderModel> CreateAsync(CartModel cartModel);
        Task RemoveAsync(Guid id);
        OrderResponseModel Filter(OrderRequestModel filter);
    }
}
