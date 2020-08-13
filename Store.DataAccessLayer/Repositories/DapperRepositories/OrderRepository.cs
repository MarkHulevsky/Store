using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Configuration;
using Store.DataAccess.Entities.Constants;
using Store.DataAccess.Filters.ResponseFulters;
using Store.DataAccess.Repositories.Base;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Filters;
using Store.DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Store.DataAccessLayer.Entities.Enums.Enums;

namespace Store.DataAccess.Repositories.DapperRepositories
{
    public class OrderRepository : BaseDapperRepository<Order>, IOrderRepository
    {
        public OrderRepository(IConfiguration configuration) : base(configuration)
        {
            tableName = Constants.orderTableName;
        }

        public async Task AddToPaymentAsync(Guid paymentId, Guid orderId)
        {
            var order = await _dbContext.GetAsync<Order>(orderId);
            var payment = await _dbContext.GetAsync<Payment>(paymentId);
            if (order != null && payment != null)
            {
                order.PaymentId = paymentId;
                order.Status = DataAccessLayer.Entities.Enums.Enums.OrderStatus.Paid;
                var query = $"UPDATE {tableName} SET PaymentId = '{order.PaymentId}', Status = {(int)order.Status}" +
                    $"WHERE Id = '{order.Id}'";
                await _dbContext.ExecuteAsync(query);
            }
        }

        public override async Task<Order> CreateAsync(Order model)
        {
            model.PaymentId = Guid.Empty;
            model.Id = Guid.NewGuid();
            model.Description = string.Empty;
            var creationDateString = DateTime.Now.ToUniversalTime().ToString("yyyyMMdd");
            model.Status = OrderStatus.Unpaid;
            var query = $"INSERT INTO {tableName} " +
                $"(Id, Description, UserId, PaymentId, Status, CreationDate, IsRemoved) " +
                $"OUTPUT INSERTED.Id " +
                $"VALUES ('{model.Id}' ,'{model.Description}', '{model.UserId}', '{model.PaymentId}', " +
                $"{(int)model.Status}, '{creationDateString}', 0)";

            model.Id = await _dbContext.QueryFirstOrDefaultAsync<Guid>(query);
            return model;
        }

        public OrderResponseFilter Filter(OrderRequestFilter filter)
        {
            var query = $"SELECT * FROM {tableName} WHERE IsRemoved != 1";
            var querybaleOrders = _dbContext.Query<Order>(query).AsQueryable();

            var uQuery = new List<Order>().AsQueryable();
            foreach (var status in filter.OrderStatuses)
            {
                uQuery = uQuery.Concat(querybaleOrders.Where(o => o.Status == status));
            }

            querybaleOrders = uQuery;
            querybaleOrders = querybaleOrders.OrderBy(filter.PropName, $"{filter.SortType}");
            var orders = querybaleOrders.Skip(filter.Paging.CurrentPage * filter.Paging.ItemsCount)
                .Take(filter.Paging.ItemsCount).ToList();

            query = $"SELECT COUNT(*) FROM {tableName} WHERE IsRemoved = 0";
            var totalCount = _dbContext.Query<int>(query).FirstOrDefault();
            var result = new OrderResponseFilter
            {
                Orders = orders,
                TotalCount = totalCount
            };
            return result;
        }

        public List<OrderItem> GetOrderItems(Guid orderId)
        {
            var query = $"SELECT * FROM OrderItems WHERE OrderId = '{orderId}'";
            var orderItems = _dbContext.Query<OrderItem>(query).ToList();
            return orderItems;
        }

        public async Task<List<Order>> GetUserOrdersAsync(Guid userId)
        {
            var query = $"SELECT * FROM {tableName} WHERE UserId = '{userId}'";
            var orders = _dbContext.Query<Order>(query).ToList();
            return orders;
        }
    }
}
