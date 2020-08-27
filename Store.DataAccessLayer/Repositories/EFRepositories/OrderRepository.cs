using Microsoft.EntityFrameworkCore;
using Store.DataAccess.AppContext;
using Store.DataAccess.Entities;
using Store.DataAccess.Filters;
using Store.DataAccess.Filters.ResponseFulters;
using Store.DataAccess.Repositories.Base;
using Store.DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Shared.Enums.Enums;

namespace Store.DataAccess.Repositories.EFRepositories
{
    public class OrderRepository : BaseEFRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task AddToPaymentAsync(Guid paymentId, Guid orderId)
        {
            var order = await DbSet.FirstOrDefaultAsync(o => o.Id == orderId);
            if (order == null)
            {
                return;
            }
            order.PaymentId = paymentId;
            order.Status = OrderStatus.Paid;
            await UpdateAsync(order);
        }

        public override async Task<Order> CreateAsync(Order model)
        {
            var entity = await DbSet.FirstOrDefaultAsync(entity => entity.Id == model.Id);
            if (entity != null)
            {
                return entity;
            }
            model.Status = OrderStatus.Unpaid;
            model.CreationDate = DateTime.UtcNow;
            var result = await DbSet.AddAsync(model);
            await SaveChangesAsync();
            model.Id = result.Entity.Id;
            return model;
        }

        public OrderResponseDataModel Filter(OrderRequestDataModel filter)
        {
            var query = DbSet.Include(order => order.User)
                .Include(order => order.OrderItems)
                .ThenInclude(orderItem => orderItem.PrintingEdition)
                .Where(o => !o.IsRemoved);

            var subquery = new List<Order>().AsQueryable();
            foreach (var status in filter.OrderStatuses)
            {
                subquery = subquery.Concat(query.Where(o => o.Status == status));
            }
            query = subquery;
            query = query.OrderBy(filter.SortPropertyName, $"{filter.SortType}");
            var orders = query.Skip(filter.Paging.CurrentPage * filter.Paging.ItemsCount)
                .Take(filter.Paging.ItemsCount).ToList();
            var result = new OrderResponseDataModel
            {
                Orders = orders,
                TotalCount = DbSet.Where(o => !o.IsRemoved).Count()
            };
            return result;
        }

        public async Task<List<Order>> GetUserOrdersAsync(Guid userId)
        {
            var orders = await DbSet.Include(order => order.OrderItems)
                .ThenInclude(orderItem => orderItem.PrintingEdition)
                .Where(order => order.UserId == userId).ToListAsync();
            return orders;
        }

    }
}
