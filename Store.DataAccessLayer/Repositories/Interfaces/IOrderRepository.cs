using Store.DataAccess.Entities;
using Store.DataAccess.Filters;
using Store.DataAccess.Filters.ResponseFulters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.DataAccess.Repositories.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task AddToPaymentAsync(Guid paymentId, Guid orderId);
        Task<List<Order>> GetUserOrdersAsync(Guid userId);
        Task<OrderResponseDataModel> FilterAsync(OrderRequestDataModel filter);
    }
}
