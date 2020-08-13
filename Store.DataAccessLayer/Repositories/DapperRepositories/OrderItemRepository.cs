using Dapper;
using Microsoft.Extensions.Configuration;
using Store.DataAccess.Entities.Constants;
using Store.DataAccess.Repositories.Base;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace Store.DataAccess.Repositories.DapperRepositories
{
    public class OrderItemRepository : BaseDapperRepository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(IConfiguration configuration) : base(configuration)
        {
            tableName = Constants.orderItemTableName;
        }

        public override async Task<OrderItem> CreateAsync(OrderItem model)
        {
            model.Id = Guid.NewGuid();
            model.CreationDate = DateTime.Now;
            var query = $"INSERT INTO {tableName} (Id, Amount, PrintingEditionId, OrderId, Count," +
                $" IsRemoved, CreationDate) " +
                $"VALUES ('{model.Id}', {model.Amount}, '{model.PrintingEditionId}', '{model.OrderId}', " +
                $"{model.Count}, 0, '{model.CreationDate}')";
            var result = await _dbContext.QueryFirstOrDefaultAsync<OrderItem>(query);
            return result;
        }
    }
}
