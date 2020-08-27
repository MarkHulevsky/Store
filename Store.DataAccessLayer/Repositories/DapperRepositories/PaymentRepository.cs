using Dapper;
using Microsoft.Extensions.Configuration;
using Store.DataAccess.Entities;
using Store.DataAccess.Repositories.Base;
using Store.DataAccess.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace Store.DataAccess.Repositories.DapperRepositories
{
    public class PaymentRepository : BaseDapperRepository<Payment>, IPaymentRepository
    {
        public const string PAYMENTS_TABLE_NAME = "Payments";
        public PaymentRepository(IConfiguration configuration) : base(configuration)
        {
            tableName = PAYMENTS_TABLE_NAME;
        }

        public override async Task<Payment> CreateAsync(Payment model)
        {
            var query = $"INSERT INTO {tableName} " +
                $"(Id, IsRemoved, CreationDate, TransactionId) " +
                $"VALUES ('{model.Id}', 0, '{model.CreationDate.ToUniversalTime().ToString("yyyyMMdd")}', '{model.TransactionId}' )";
            await _dbContext.QueryFirstOrDefaultAsync(query);
            return model;
        }
    }
}
