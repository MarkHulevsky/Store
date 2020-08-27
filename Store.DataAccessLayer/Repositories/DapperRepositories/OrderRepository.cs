using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Configuration;
using Store.DataAccess.Entities;
using Store.DataAccess.Filters;
using Store.DataAccess.Filters.ResponseFulters;
using Store.DataAccess.Models.Constants;
using Store.DataAccess.Repositories.Base;
using Store.DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Shared.Enums.Enums;

namespace Store.DataAccess.Repositories.DapperRepositories
{
    public class OrderRepository : BaseDapperRepository<Order>, IOrderRepository
    {
        public const string ORDERS_TABLE_NAME = "Orders";
        public OrderRepository(IConfiguration configuration) : base(configuration)
        {
            tableName = ORDERS_TABLE_NAME;
        }

        public async Task AddToPaymentAsync(Guid paymentId, Guid orderId)
        {
            var order = await _dbContext.GetAsync<Order>(orderId);
            var payment = await _dbContext.GetAsync<Payment>(paymentId);
            if (order != null && payment != null)
            {
                order.PaymentId = paymentId;
                order.Status = OrderStatus.Paid;
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

        public OrderResponseDataModel Filter(OrderRequestDataModel filter)
        {
            var query = $"SELECT * FROM {tableName} " +
                $"LEFT JOIN {Constants.USERS_TABLE_NAME} ON {tableName}.UserId = {Constants.USERS_TABLE_NAME}.Id " +
                $"LEFT JOIN {Constants.ORDER_ITEMS_TABLE_NAME} ON {Constants.ORDER_ITEMS_TABLE_NAME}.OrderId = Orders.Id " +
                $"LEFT JOIN {Constants.PRINTING_EDITIONS_TABLE_NAME} " +
                $"ON {Constants.ORDER_ITEMS_TABLE_NAME}.PrintingEditionId = {Constants.PRINTING_EDITIONS_TABLE_NAME}.Id " +
                $"WHERE {tableName}.IsRemoved != 1";
            var orderDictionary = new Dictionary<Guid, Order>();
            var querybaleOrders = _dbContext.Query<Order, User, OrderItem, PrintingEdition, Order>(
                query, (order, user, orderItem, printingEdition) =>
                {
                    var orderEntry = new Order();
                    if (!orderDictionary.TryGetValue(order.Id, out orderEntry))
                    {
                        orderEntry = order;
                        order.OrderItems = new List<OrderItem>();
                        orderDictionary.Add(order.Id, orderEntry);
                    }
                    orderEntry.User = user;
                    orderItem.PrintingEdition = printingEdition;
                    orderEntry.OrderItems.Add(orderItem);
                    return orderEntry;
                })
                .Distinct()
                .AsQueryable();

            var subquery = new List<Order>().AsQueryable();
            foreach (var status in filter.OrderStatuses)
            {
                subquery = subquery.Concat(querybaleOrders.Where(o => o.Status == status));
            }

            querybaleOrders = subquery;
            querybaleOrders = querybaleOrders.OrderBy(filter.SortPropertyName, $"{filter.SortType}");
            var orders = querybaleOrders.Skip(filter.Paging.CurrentPage * filter.Paging.ItemsCount)
                .Take(filter.Paging.ItemsCount).ToList();

            query = $"SELECT COUNT(*) FROM {tableName} WHERE IsRemoved = 0";
            var totalCount = _dbContext.Query<int>(query).FirstOrDefault();
            var result = new OrderResponseDataModel
            {
                Orders = orders,
                TotalCount = totalCount
            };
            return result;
        }

        public async Task<List<Order>> GetUserOrdersAsync(Guid userId)
        {
            var query = $"SELECT * FROM {tableName} " +
                $"LEFT JOIN {Constants.ORDER_ITEMS_TABLE_NAME} ON {Constants.ORDER_ITEMS_TABLE_NAME}.OrderId = {tableName}.Id " +
                $"LEFT JOIN {Constants.PRINTING_EDITIONS_TABLE_NAME} " +
                $"ON {Constants.PRINTING_EDITIONS_TABLE_NAME}.Id = {Constants.ORDER_ITEMS_TABLE_NAME}.PrintingEditionId " +
                $"WHERE UserId = '{userId}'";
            var orderDictionary = new Dictionary<Guid, Order>();
            var orders = await _dbContext.QueryAsync<Order, OrderItem, PrintingEdition, Order>(
                query, (order, orderItem, printingEdition) =>
                {
                    var orderEntry = new Order();
                    if (!orderDictionary.TryGetValue(order.Id, out orderEntry))
                    {
                        orderEntry = order;
                        orderEntry.OrderItems = new List<OrderItem>();
                        orderDictionary.Add(orderEntry.Id, orderEntry);
                    }
                    orderItem.PrintingEdition = printingEdition;
                    orderEntry.OrderItems.Add(orderItem);
                    return orderEntry;
                });
            orders = orders.Distinct();
            return orders.ToList();
        }
    }
}
