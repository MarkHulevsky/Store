using Microsoft.EntityFrameworkCore;
using Store.DataAccess.Filters.ResponseFulters;
using Store.DataAccessLayer.AppContext;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Filters;
using Store.DataAccessLayer.Repositories.Base;
using Store.DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Repositories.EFRepositories
{
    public class OrderRepository : BaseEFRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task AddToPaymentAsync(Guid paymentId, Guid orderId)
        {
            var order = _dbContext.Orders.FirstOrDefault(o => o.Id == orderId);
            var payment = _dbContext.Payments.FirstOrDefault(p => p.Id == paymentId);
            if (order != null && payment != null)
            {
                order.PaymentId = paymentId;
                order.Status = Entities.Enums.Enums.OrderStatus.Paid;
                await UpdateAsync(order);
            }
        }

        public override async Task<Order> CreateAsync(Order model)
        {
            var entity = await _dbContext.Orders.FirstOrDefaultAsync(entity => entity.Id == model.Id);
            if (entity == null)
            {
                model.CreationDate = DateTime.UtcNow;
                var result = await _dbContext.Orders.AddAsync(model);
                await _dbContext.SaveChangesAsync();
                model.Id = result.Entity.Id;
                return model;
            }
            return entity;
        }

        public OrderResponseFilter Filter(OrderRequestFilter filter)
        {
            var query = _dbContext.Orders.Where(o => !o.IsRemoved);
            var uQuery = new List<Order>().AsQueryable();
            foreach (var status in filter.OrderStatuses)
            {
                uQuery = uQuery.Concat(query.Where(o => o.Status == status));
            }
            query = uQuery;
            query = query.OrderBy(filter.PropName, $"{filter.SortType}");
            var orders = query.Skip(filter.Paging.CurrentPage * filter.Paging.ItemsCount)
                .Take(filter.Paging.ItemsCount).ToList();
            var result = new OrderResponseFilter
            {
                Orders = orders,
                TotalCount = _dbContext.Orders.Where(o => !o.IsRemoved).Count()
            };
            return result;
        }

        public async Task<List<Order>> GetUserOrdersAsync(Guid userId)
        {
            var orders = await _dbContext.Orders.Where(order => order.UserId == userId).ToListAsync();
            return orders;
        }

        public List<OrderItem> GetOrderItems(Guid orderId)
        {
            var orderItems = _dbContext.OrderItems.Where(orderItem => orderItem.OrderId == orderId).ToList();
            return orderItems;
        }
    }
}
