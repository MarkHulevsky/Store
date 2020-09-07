using Dapper;
using Microsoft.Extensions.Configuration;
using Store.DataAccess.Entities;
using Store.DataAccess.Filters;
using Store.DataAccess.Filters.ResponseFulters;
using Store.DataAccess.Models.Constants;
using Store.DataAccess.Repositories.Base;
using Store.DataAccess.Repositories.Interfaces;
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
            tableName = Constants.AUTHORS_TABLE_NAME;
        }

        public override async Task<Author> CreateAsync(Author author)
        {
            var query = $"INSERT INTO {tableName} (Id, CreationDate, IsRemoved, Name) " +
                $"OUTPUT INSERTED.Id " +
                $"VALUES ('{Guid.NewGuid()}', '{author.CreationDate.ToUniversalTime():yyyyMMdd}', " +
                $"{Convert.ToInt32(author.IsRemoved)}, '{author.Name}')";
            author.Id = await _dbContext.QueryFirstOrDefaultAsync<Guid>(query);
            return author;
        }

        public async Task<AuthorResponseDataModel> FilterAsync(AuthorRequestDataModel authorRequestDataModel)
        {
            var query = $"SELECT * FROM {tableName} LEFT JOIN (" +
                $"SELECT {Constants.AUTHOR_IN_PRINTING_EDITIONS_TABLE_NAME}.AuthorId, " +
                $"{Constants.AUTHOR_IN_PRINTING_EDITIONS_TABLE_NAME}.PrintingEditionId FROM {Constants.AUTHOR_IN_PRINTING_EDITIONS_TABLE_NAME} " +
                $") AS {Constants.AUTHOR_IN_PRINTING_EDITIONS_TABLE_NAME} " +
                $"ON {Constants.AUTHOR_IN_PRINTING_EDITIONS_TABLE_NAME}.AuthorId = {tableName}.Id " +
                $"LEFT JOIN {Constants.PRINTING_EDITIONS_TABLE_NAME} " +
                $"ON {Constants.AUTHOR_IN_PRINTING_EDITIONS_TABLE_NAME}.PrintingEditionId = {Constants.PRINTING_EDITIONS_TABLE_NAME}.Id " +
                $"WHERE {tableName}.IsRemoved != 1";

            var authorsDictionary = new Dictionary<Guid, Author>();
            var authors = await _dbContext.QueryAsync<Author, PrintingEdition, Author>(
                query, (author, printingEdition) =>
                {
                    var authorEntry = new Author();
                    if (!authorsDictionary.TryGetValue(author.Id, out authorEntry))
                    {
                        authorEntry = author;
                        authorsDictionary.Add(authorEntry.Id, authorEntry);
                    }
                    authorEntry.PrintingEditions.Add(printingEdition);
                    return authorEntry;
                });
            authors = authors
                .Distinct()
                .AsQueryable()
                .OrderBy(authorRequestDataModel.SortPropertyName, authorRequestDataModel.SortType.ToString())
                .Skip(authorRequestDataModel.Paging.ItemsCount * authorRequestDataModel.Paging.CurrentPage)
                .Take(authorRequestDataModel.Paging.ItemsCount)
                .ToList();

            query = $"SELECT COUNT(*) FROM {tableName} WHERE IsRemoved != 1";
            var totalCount = await _dbContext.QueryFirstOrDefaultAsync<int>(query);
            var result = new AuthorResponseDataModel
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

        public override async Task<Author> UpdateAsync(Author author)
        {
            var query = $"UPDATE {tableName} SET Name = '{author.Name}' WHERE Id = '{author.Id}'";
            var authorEntry = await _dbContext.QueryFirstOrDefaultAsync<Author>(query);
            return authorEntry;
        }
    }
}
