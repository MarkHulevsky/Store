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

        public Task AddRangeAsync(List<OrderItem> orderItems)
        {
            throw new System.NotImplementedException();
        }

        public override async Task<OrderItem> CreateAsync(OrderItem model)
        {
            var query = $"INSERT INTO {tableName} (Id, Amount, PrintingEditionId, OrderId, Count," +
                $" IsRemoved, CreationDate) " +
                $"VALUES ('{model.Id}', {model.Amount}, '{model.PrintingEditionId}', '{model.OrderId}', " +
                $"{model.Count}, 0, '{model.CreationDate.ToUniversalTime().ToString("yyyyMMdd")}')";
            var result = await _dbContext.QueryFirstOrDefaultAsync<OrderItem>(query);
            return result;
        }

    }
}
