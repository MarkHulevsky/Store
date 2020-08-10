using Dapper;
using Microsoft.Extensions.Configuration;
using Store.DataAccess.Repositories.Base;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Repositories.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Store.DataAccess.Repositories.DapperRepositories
{
    public class AuthorInPrintingEditionRepository : BaseDapperRepository<AuthorInPrintingEdition>,
        IAuthorInPrintingEditionRepository
    {
        public AuthorInPrintingEditionRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public override async Task<AuthorInPrintingEdition> CreateAsync(AuthorInPrintingEdition model)
        {
            var query = $"SELECT * FROM {TableName} " +
                $"WHERE AuthorId = '{model.AuthorId}' AND PrintingEditionId = '{model.PrintingEditionId}'";
            var entity = await _dbContext.QueryFirstOrDefaultAsync<AuthorInPrintingEdition>(query);
            if (entity == null)
            {
                query = $"INSERT INTO {TableName} (Id ,AuthorId, PrintingEditionId, CreationDate, IsRemoved) " +
                    $"VALUES ('{Guid.NewGuid()}' ,'{model.AuthorId}', '{model.PrintingEditionId}', '{DateTime.Now}', 0)";
                await _dbContext.QueryAsync<AuthorInPrintingEdition>(query);
            }
            return model;
        }
    }
}
