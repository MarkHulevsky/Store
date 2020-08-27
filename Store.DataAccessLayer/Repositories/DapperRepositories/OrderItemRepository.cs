using Dapper;
using Microsoft.Extensions.Configuration;
using Store.DataAccess.Entities;
using Store.DataAccess.Models.Constants;
using Store.DataAccess.Repositories.Base;
using Store.DataAccess.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace Store.DataAccess.Repositories.DapperRepositories
{
    public class OrderItemRepository : BaseDapperRepository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(IConfiguration configuration) : base(configuration)
        {
            tableName = Constants.ORDER_ITEMS_TABLE_NAME;
        }

        public override async Task<OrderItem> CreateAsync(OrderItem model)
        {
            model.Id = Guid.NewGuid();
            var creationDateString = DateTime.Now.ToUniversalTime().ToString("yyyyMMdd");
            var query = $"INSERT INTO {tableName} (Id, Amount, PrintingEditionId, OrderId, Count," +
                $" IsRemoved, CreationDate) " +
                $"VALUES ('{model.Id}', {model.Amount}, '{model.PrintingEditionId}', '{model.OrderId}', " +
                $"{model.Count}, 0, '{creationDateString}')";
            var result = await _dbContext.QueryFirstOrDefaultAsync<OrderItem>(query);
            return result;
        }
    }
}
