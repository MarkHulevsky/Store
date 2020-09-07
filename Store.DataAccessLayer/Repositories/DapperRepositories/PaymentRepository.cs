using Dapper;
using Microsoft.Extensions.Configuration;
using Store.DataAccess.Entities;
using Store.DataAccess.Repositories.Base;
using Store.DataAccess.Repositories.Interfaces;
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

        public override async Task<Payment> CreateAsync(Payment payment)
        {
            var query = $"INSERT INTO {tableName} " +
                $"(Id, IsRemoved, CreationDate, TransactionId) " +
                $"VALUES ('{payment.Id}', 0, '{payment.CreationDate.ToUniversalTime().ToString("yyyyMMdd")}', '{payment.TransactionId}' )";
            await _dbContext.QueryFirstOrDefaultAsync(query);
            return payment;
        }
    }
}
