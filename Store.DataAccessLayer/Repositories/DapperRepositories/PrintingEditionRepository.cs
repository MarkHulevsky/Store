using Dapper;
using Microsoft.Extensions.Configuration;
using Store.DataAccess.Filters.ResponseFulters;
using Store.DataAccess.Repositories.Base;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Filters;
using Store.DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.DataAccess.Repositories.DapperRepositories
{
    public class PrintingEditionRepository : BaseDapperRepository<PrintingEdition>, IPrintingEditionRepository
    {
        public PrintingEditionRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public PrintingEditionResponseFilter Filter(PrintingEditionsRequestFilter filter)
        {
            var query = $"SELECT * FROM {TableName} WHERE Title LIKE '%{filter.SearchString}%' AND IsRemoved = 0";
            var queryblePrintingEditions = _dbContext.Query<PrintingEdition>(query).AsQueryable();

            var uQuery = new List<PrintingEdition>().AsQueryable();

            foreach (var type in filter.Types)
            {
                uQuery = uQuery.Concat(queryblePrintingEditions.Where(pe => pe.Type == type));
            }

            queryblePrintingEditions = uQuery;

            if (filter.MaxPrice > filter.MinPrice && filter.MaxPrice != filter.MinPrice)
            {
                queryblePrintingEditions = queryblePrintingEditions.Where(pe => pe.Price <= filter.MaxPrice && pe.Price >= filter.MinPrice);
            }

            queryblePrintingEditions = queryblePrintingEditions.OrderBy("Price", $"{filter.SortType}");

            var printingEditions = queryblePrintingEditions.Skip(filter.Paging.CurrentPage * filter.Paging.ItemsCount)
                .Take(filter.Paging.ItemsCount).ToList();
            query = $"SELECT COUNT(*) FROM {TableName} WHERE IsRemoved = 0";
            var totalCount = _dbContext.QueryFirstOrDefault<int>(query);

            var result = new PrintingEditionResponseFilter
            {
                PrintingEditions = printingEditions,
                TotalCount = totalCount
            };
            return result;
        }

        public override async Task<PrintingEdition> CreateAsync(PrintingEdition model)
        {
            var query = $"INSERT INTO {TableName} " +
                $"(Id, Title, Description, Price, Currency, Type, IsRemoved, CreationDate) " +
                $"OUTPUT INSERTED.Id " +
                $"VALUES ('{Guid.NewGuid()}', '{model.Title}', '{model.Description}', " +
                $"{model.Price}, {(int)model.Currency}, {(int)model.Type}, 0, '{DateTime.Now}' )";

            model.Id = await _dbContext.QueryFirstOrDefaultAsync<Guid>(query);
            return model;
        }

        public async Task<List<Author>> GetAuthorsAsync(PrintingEdition printingEdition)
        {
            var query = $"SELECT * FROM AuthorInPrintingEditions WHERE PrintingEditionId = '{printingEdition.Id}'";
            var authorInPrintingEditions = _dbContext.Query<AuthorInPrintingEdition>(query).ToList();
            var authors = new List<Author>();
            foreach (var aInPe in authorInPrintingEditions)
            {
                query = $"SELECT * FROM Authors WHERE Id = '{aInPe.AuthorId}'";
                var author = await _dbContext.QueryFirstOrDefaultAsync<Author>(query);
                authors.Add(author);
            }
            return authors;
        }

        public override async Task<PrintingEdition> UpdateAsync(PrintingEdition model)
        {
            var query = $"UPDATE {TableName} SET Title = '{model.Title}', Price = '{model.Price}'," +
                $"Type = {(int)model.Type}, Description = '{model.Description}', Currency = {(int)model.Currency} " +
                $"WHERE Id = '{model.Id}'";
            var result = await _dbContext.QueryFirstOrDefaultAsync<PrintingEdition>(query);
            return result;
        }
    }
}
