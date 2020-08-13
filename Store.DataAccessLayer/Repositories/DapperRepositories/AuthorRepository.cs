using Dapper;
using Microsoft.Extensions.Configuration;
using Store.DataAccess.Entities.Constants;
using Store.DataAccess.Filters.ResponseFulters;
using Store.DataAccess.Repositories.Base;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Filters;
using Store.DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Store.DataAccess.Repositories.DapperRepositories
{
    public class AuthorRepository : BaseDapperRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(IConfiguration configuration) : base(configuration)
        {
            tableName = Constants.authorTableName;
        }

        public override async Task<Author> CreateAsync(Author model)
        {
            model.Id = Guid.NewGuid();
            var creationDateString = DateTime.Now.ToUniversalTime().ToString("yyyyMMdd");
            model.IsRemoved = false;
            var query = $"INSERT INTO {tableName} (Id, CreationDate, IsRemoved, Name) " +
                $"OUTPUT INSERTED.Id " +
                $"VALUES ('{model.Id}', '{creationDateString}', " +
                $"{Convert.ToInt32(model.IsRemoved)}, '{model.Name}')";
            model.Id = await _dbContext.QueryFirstOrDefaultAsync<Guid>(query);
            return model;
        }

        public async Task<AuthorResponseFilter> FilterAsync(AuthorRequestFilter filter)
        {
            var query = $"SELECT * FROM {tableName} WHERE IsRemoved != 1";
            var queryableAuthors = _dbContext.Query<Author>(query).AsQueryable()
                .OrderBy(filter.PropName, filter.SortType.ToString());
            var authors = queryableAuthors.Skip(filter.Paging.ItemsCount * filter.Paging.CurrentPage)
                .Take(filter.Paging.ItemsCount).ToList();
            foreach (var author in authors)
            {
                author.PrintingEditions = await GetPrintingEditionsAsync(author);
            }
            query = $"SELECT COUNT(*) FROM {tableName} WHERE IsRemoved != 1";
            var totalCount = _dbContext.Query<int>(query).FirstOrDefault();
            var result = new AuthorResponseFilter
            {
                Authors = authors,
                TotalCount = totalCount
            };
            return result;
        }

        public async Task<Author> FindAuthorByNameAsync(string name)
        {
            var query = $"SELECT * FROM {tableName} WHERE Name = '{name}'";
            var author = await _dbContext.QueryFirstOrDefaultAsync<Author>(query);
            return author;
        }

        public override async Task<Author> UpdateAsync(Author model)
        {
            var query = $"UPDATE {tableName} SET Name = '{model.Name}' WHERE Id = '{model.Id}'";
            var author = await _dbContext.QueryFirstOrDefaultAsync<Author>(query);
            return author;
        }

        public async Task<List<PrintingEdition>> GetPrintingEditionsAsync(Author author)
        {
            var query = $"SELECT * FROM AuthorInPrintingEditions WHERE AuthorId = '{author.Id}'";
            var authorInPrintingEditions = await _dbContext.QueryAsync<AuthorInPrintingEdition>(query);
            var printingEditions = new List<PrintingEdition>();
            foreach (var aInPe in authorInPrintingEditions)
            {
                query = $"SELECT * FROM PrintingEditions WHERE Id = '{aInPe.PrintingEditionId}' and IsRemoved != 1";
                var printingEdition = await _dbContext.QueryFirstOrDefaultAsync<PrintingEdition>(query);
                if (printingEdition != null)
                {
                    printingEditions.Add(printingEdition);
                }
            }
            return printingEditions;
        }
    }
}
