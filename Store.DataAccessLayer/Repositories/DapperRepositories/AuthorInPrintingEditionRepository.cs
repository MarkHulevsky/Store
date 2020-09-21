using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Configuration;
using Store.DataAccess.Entities;
using Store.DataAccess.Models.Constants;
using Store.DataAccess.Repositories.Base;
using Store.DataAccess.Repositories.Interfaces;
using System.Collections.Generic;
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

        public async Task AddRangeAsync(List<AuthorInPrintingEdition> authorInPrintingEditions)
        {
            await _dbContext.InsertAsync(authorInPrintingEditions);
        }

        public override async Task<AuthorInPrintingEdition> CreateAsync(AuthorInPrintingEdition authorInPrintingEdition)
        {
            var query = $"SELECT * FROM {tableName} " +
                $"WHERE AuthorId = '{authorInPrintingEdition.AuthorId}' AND PrintingEditionId = '{authorInPrintingEdition.PrintingEditionId}'";
            var entity = await _dbContext.QueryFirstOrDefaultAsync<AuthorInPrintingEdition>(query);
            if (entity != null)
            {
                return authorInPrintingEdition;
            }
            await _dbContext.InsertAsync(authorInPrintingEdition);
            return authorInPrintingEdition;
        }
    }
}
