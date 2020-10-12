using Store.BuisnessLogic.Models.Filters;
using Store.BuisnessLogic.Models.Filters.ResponseFilters;
using Store.BuisnessLogic.Models.Orders;
using Store.BuisnessLogic.Models.Payments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.BuisnessLogic.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<string>> PayOrderAsync(PaymentModel paymentModel);
        Task<List<OrderModel>> GetUserOrdersAsync();
        Task<OrderModel> CreateAsync(CartModel cartModel);
        Task RemoveAsync(string id);
        Task<OrderResponseModel> FilterAsync(OrderRequestModel orderRequestModel);
    }
}
