using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Filters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Repositories.Interfaces
{
    public interface IOrderRepository: IRepository<Order>
    {
        Task AddToPaymentAsync(Guid paymentId, Guid orderId);
        Task<List<Order>> GetUserOrdersAsync(Guid userId);
        Task<List<Order>> FilterAsync(OrderRequestFilter filter);
    }
}
