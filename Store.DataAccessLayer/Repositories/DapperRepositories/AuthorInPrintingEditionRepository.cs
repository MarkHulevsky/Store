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
    public class AuthorInPrintingEditionRepository : BaseDapperRepository<AuthorInPrintingEdition>,
        IAuthorInPrintingEditionRepository
    {
        public AuthorInPrintingEditionRepository(IConfiguration configuration) : base(configuration)
        {
            tableName = Constants.AUTHOR_IN_PRINTING_EDITIONS_TABLE_NAME;
        }

        public override async Task<AuthorInPrintingEdition> CreateAsync(AuthorInPrintingEdition model)
        {
            var creationDateString = DateTime.Now.ToUniversalTime().ToString("yyyyMMdd");
            model.Id = Guid.NewGuid();
            var query = $"SELECT * FROM {tableName} " +
                $"WHERE AuthorId = '{model.AuthorId}' AND PrintingEditionId = '{model.PrintingEditionId}'";
            var entity = await _dbContext.QueryFirstOrDefaultAsync<AuthorInPrintingEdition>(query);
            if (entity != null)
            {
                return model;
            }
            query = $"INSERT INTO {tableName} (Id, AuthorId, PrintingEditionId, CreationDate, IsRemoved) " +
                $"VALUES ('{model.Id}' ,'{model.AuthorId}', '{model.PrintingEditionId}'," +
                $" '{creationDateString}', 0)";
            await _dbContext.QueryAsync<AuthorInPrintingEdition>(query);
            return model;
        }
    }
}
