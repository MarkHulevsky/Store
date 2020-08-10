using Dapper;
using Microsoft.Extensions.Configuration;
using Store.DataAccess.Repositories.Base;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace Store.DataAccess.Repositories.DapperRepositories
{
    public class PaymentRepository: BaseDapperRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(IConfiguration configuration): base(configuration) {}

        public override async Task<Payment> CreateAsync(Payment model)
        {
            model.Id = Guid.NewGuid();
            model.CreationDate = DateTime.Now;
            var query = $"INSERT INTO {TableName} " +
                $"(Id, IsRemoved, CreationDate, TransactionId) " +
                $"VALUES ('{model.Id}', 0, '{model.CreationDate}', '{model.TransactionId}' )";
            await _dbContext.QueryFirstOrDefaultAsync(query);
            return model;
        }
    }
}
