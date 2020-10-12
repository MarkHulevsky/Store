using Dapper;
using Microsoft.Data.SqlClient;
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

namespace Store.DataAccess.Repositories.DapperRepositories
{
    public class OrderRepository : BaseDapperRepository<Order>, IOrderRepository
    {
        public const string ORDERS_TABLE_NAME = "Orders";
        public OrderRepository(IConfiguration configuration) : base(configuration)
        {
            tableName = ORDERS_TABLE_NAME;
        }

        public async Task<OrderResponseDataModel> FilterAsync(OrderRequestDataModel orderRequestDataModel)
        {
            var query = $@"SELECT * FROM (
                        	SELECT * FROM {tableName} WHERE {tableName}.IsRemoved != 1
                        	ORDER BY {tableName}.CreationDate
                        	OFFSET {orderRequestDataModel.Paging.ItemsCount * orderRequestDataModel.Paging.CurrentPage} ROWS 
                            FETCH NEXT {orderRequestDataModel.Paging.ItemsCount} ROWS ONLY
                        ) AS {tableName}
                        LEFT JOIN {Constants.USERS_TABLE_NAME} ON {tableName}.UserId = {Constants.USERS_TABLE_NAME}.Id
                        LEFT JOIN {Constants.ORDER_ITEMS_TABLE_NAME} ON {Constants.ORDER_ITEMS_TABLE_NAME}.OrderId = {tableName}.Id
                        LEFT JOIN {Constants.PRINTING_EDITIONS_TABLE_NAME} ON {Constants.ORDER_ITEMS_TABLE_NAME}.PrintingEditionId 
                            = {Constants.PRINTING_EDITIONS_TABLE_NAME}.Id";
            using (var dbContext = new SqlConnection(connectionString))
            {
                await dbContext.OpenAsync();
                var orders = await dbContext.QueryAsync<Order, User, OrderItem, PrintingEdition, Order>(
                query, (order, user, orderItem, printingEdition) =>
                {
                    order.User = user;
                    if (orderItem != null)
                    {
                        orderItem.PrintingEdition = printingEdition;
                    }
                    order.OrderItems.Add(orderItem);
                    return order;
                });

                var querybaleOrders = orders
                    .GroupBy(order => order.Id)
                    .Select(group =>
                    {
                        var result = group.FirstOrDefault();
                        result.OrderItems = group.Select(order => order.OrderItems.SingleOrDefault()).ToList();
                        return result;
                    });

                var subquery = new List<Order>().AsQueryable();
                foreach (var status in orderRequestDataModel.OrderStatuses)
                {
                    subquery = subquery.Concat(querybaleOrders.Where(o => o.Status == status));
                }
                querybaleOrders = subquery;
                orders = querybaleOrders.ToList();
                var totalCount = orders.Count();
                var result = new OrderResponseDataModel
                {
                    Orders = orders,
                    TotalCount = totalCount
                };
                return result;
            }
        }

        public async Task<List<Order>> GetUserOrdersAsync(Guid userId)
        {
            var query = $"SELECT * FROM {tableName} " +
                $"LEFT JOIN {Constants.ORDER_ITEMS_TABLE_NAME} ON {Constants.ORDER_ITEMS_TABLE_NAME}.OrderId = {tableName}.Id " +
                $"LEFT JOIN {Constants.PRINTING_EDITIONS_TABLE_NAME} " +
                $"ON {Constants.PRINTING_EDITIONS_TABLE_NAME}.Id = {Constants.ORDER_ITEMS_TABLE_NAME}.PrintingEditionId " +
                $"WHERE UserId = '{userId}'";
            using (var dbContext = new SqlConnection(connectionString))
            {
                await dbContext.OpenAsync();
                var orders = await dbContext.QueryAsync<Order, OrderItem, PrintingEdition, Order>(
                query, (order, orderItem, printingEdition) =>
                {
                    if (orderItem != null)
                    {
                        orderItem.PrintingEdition = printingEdition;
                    }
                    order.OrderItems.Add(orderItem);
                    return order;
                });
                var querybaleOrders = orders
                    .GroupBy(order => order.Id)
                    .Select(group =>
                    {
                        var result = group.FirstOrDefault();
                        result.OrderItems = group.Select(order => order.OrderItems.SingleOrDefault()).ToList();
                        return result;
                    });
                orders = querybaleOrders.ToList();
                return orders.ToList();
            }
        }
    }
}
