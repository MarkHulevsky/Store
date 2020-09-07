using Dapper;
using Microsoft.Extensions.Configuration;
using Store.DataAccess.Entities;
using Store.DataAccess.Models.Constants;
using Store.DataAccess.Repositories.Base;
using Store.DataAccess.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.DataAccess.Repositories.DapperRepositories
{
    public class OrderItemRepository : BaseDapperRepository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(IConfiguration configuration) : base(configuration)
        {
            tableName = Constants.ORDER_ITEMS_TABLE_NAME;
        }

        public async Task AddRangeAsync(List<OrderItem> orderItems)
        {
            var query = $"INSERT INTO {tableName} (Id, Amount, PrintingEditionId, OrderId, Count, IsRemoved, CreationDate) " +
                $"VALUES(@Id, @Amount, @PrintingEditionId, @OrderId, @Count, @IsRemoved, @CreationDate)";
            await _dbContext.ExecuteAsync(query, orderItems);
        }

        public override async Task<OrderItem> CreateAsync(OrderItem orderItem)
        {
            var query = $"INSERT INTO {tableName} (Id, Amount, PrintingEditionId, OrderId, Count," +
                $" IsRemoved, CreationDate) " +
                $"VALUES ('{orderItem.Id}', {orderItem.Amount}, '{orderItem.PrintingEditionId}', '{orderItem.OrderId}', " +
                $"{orderItem.Count}, 0, '{orderItem.CreationDate.ToUniversalTime():yyyyMMdd}')";
            var result = await _dbContext.QueryFirstOrDefaultAsync<OrderItem>(query);
            return result;
        }

    }
}
