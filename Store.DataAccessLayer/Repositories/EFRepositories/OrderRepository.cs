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

namespace Store.DataAccess.Repositories.EFRepositories
{
    public class OrderRepository : BaseEFRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationContext context) : base(context)
        {
        }

        public override async Task<Order> CreateAsync(Order order)
        {
            var entity = await DbSet.FirstOrDefaultAsync(entity => entity.Id == order.Id);
            if (entity != null)
            {
                return entity;
            }
            var result = await DbSet.AddAsync(order);
            await SaveChangesAsync();
            return order;
        }

        public async Task<OrderResponseDataModel> FilterAsync(OrderRequestDataModel orderRequestDataModel)
        {
            var query = DbSet.Include(order => order.User)
                .Include(order => order.OrderItems)
                .ThenInclude(orderItem => orderItem.PrintingEdition)
                .Where(o => !o.IsRemoved);
            var totalCount = await query.CountAsync();
            var subquery = new List<Order>().AsQueryable();
            foreach (var status in orderRequestDataModel.OrderStatuses)
            {
                subquery = subquery.Concat(query.Where(o => o.Status == status));
            }
            query = subquery;
            query = query
                .OrderBy(orderRequestDataModel.SortPropertyName, $"{orderRequestDataModel.SortType}")
                .Skip(orderRequestDataModel.Paging.CurrentPage * orderRequestDataModel.Paging.ItemsCount)
                .Take(orderRequestDataModel.Paging.ItemsCount);
            var orders = query.ToList();
            var result = new OrderResponseDataModel
            {
                Orders = orders,
                TotalCount = totalCount
            };
            return result;
        }

        public async Task<List<Order>> GetUserOrdersAsync(Guid userId)
        {
            var orders = await DbSet
                .Include(order => order.OrderItems)
                .ThenInclude(orderItem => orderItem.PrintingEdition)
                .Where(order => order.UserId == userId)
                .ToListAsync();
            return orders;
        }

    }
}
