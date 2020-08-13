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
    public class PaymentRepository : BaseDapperRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(IConfiguration configuration) : base(configuration) 
        {
            tableName = Constants.paymentTableName;
        }

        public override async Task<Payment> CreateAsync(Payment model)
        {
            model.Id = Guid.NewGuid();
            var creationDateString = DateTime.Now.ToUniversalTime().ToString("yyyyMMdd");
            var query = $"INSERT INTO {tableName} " +
                $"(Id, IsRemoved, CreationDate, TransactionId) " +
                $"VALUES ('{model.Id}', 0, '{creationDateString}', '{model.TransactionId}' )";
            await _dbContext.QueryFirstOrDefaultAsync(query);
            return model;
        }
    }
}
