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
    public class AuthorInPrintingEditionRepository : BaseDapperRepository<AuthorInPrintingEdition>,
        IAuthorInPrintingEditionRepository
    {
        public AuthorInPrintingEditionRepository(IConfiguration configuration) : base(configuration)
        {
            tableName = Constants.authorInPrintingEditionTableName;
        }

        public override async Task<AuthorInPrintingEdition> CreateAsync(AuthorInPrintingEdition model)
        {
            var creationDateString = DateTime.Now.ToUniversalTime().ToString("yyyyMMdd");
            var query = $"SELECT * FROM {tableName} " +
                $"WHERE AuthorId = '{model.AuthorId}' AND PrintingEditionId = '{model.PrintingEditionId}'";
            var entity = await _dbContext.QueryFirstOrDefaultAsync<AuthorInPrintingEdition>(query);
            if (entity == null)
            {
                query = $"INSERT INTO {tableName} (Id ,AuthorId, PrintingEditionId, CreationDate, IsRemoved) " +
                    $"VALUES ('{Guid.NewGuid()}' ,'{model.AuthorId}', '{model.PrintingEditionId}'," +
                    $" '{creationDateString}', 0)";
                await _dbContext.QueryAsync<AuthorInPrintingEdition>(query);
            }
            return model;
        }
    }
}
