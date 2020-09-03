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
using System.Linq;
using System.Threading.Tasks;

namespace Store.DataAccess.Repositories.DapperRepositories
{
    public class PrintingEditionRepository : BaseDapperRepository<PrintingEdition>, IPrintingEditionRepository
    {
        public PrintingEditionRepository(IConfiguration configuration) : base(configuration)
        {
            tableName = Constants.PRINTING_EDITIONS_TABLE_NAME;
        }

        public async Task<PrintingEditionResponseDataModel> FilterAsync(PrintingEditionsRequestDataModel filter)
        {
            var query = $"SELECT * FROM {tableName} LEFT JOIN (" +
                $"SELECT {Constants.AUTHOR_IN_PRINTING_EDITIONS_TABLE_NAME}.PrintingEditionId, " +
                $"{Constants.AUTHOR_IN_PRINTING_EDITIONS_TABLE_NAME}.AuthorId FROM {Constants.AUTHOR_IN_PRINTING_EDITIONS_TABLE_NAME} " +
                $") AS {Constants.AUTHOR_IN_PRINTING_EDITIONS_TABLE_NAME} " +
                $"ON {Constants.AUTHOR_IN_PRINTING_EDITIONS_TABLE_NAME}.PrintingEditionId = {tableName}.Id " +
                $"LEFT JOIN {Constants.AUTHORS_TABLE_NAME} " +
                $"ON {Constants.AUTHOR_IN_PRINTING_EDITIONS_TABLE_NAME}.AuthorId = {Constants.AUTHORS_TABLE_NAME}.Id " +
                $"WHERE {tableName}.Title LIKE '%{filter.SearchString}%' AND {tableName}.IsRemoved = 0";

            var printingEditionDictionary = new Dictionary<Guid, PrintingEdition>();
            var printingEditions = await _dbContext.QueryAsync<PrintingEdition, Author, PrintingEdition>(
                query, (printingEdition, author) =>
                {
                    var printingEditionEntry = new PrintingEdition();
                    if (!printingEditionDictionary.TryGetValue(printingEdition.Id, out printingEditionEntry))
                    {
                        printingEditionEntry = printingEdition;
                        printingEditionDictionary.Add(printingEditionEntry.Id, printingEditionEntry);
                    }
                    if (author != null)
                    {
                        printingEditionEntry.Authors.Add(author);
                    }
                    return printingEditionEntry;
                });
            var queryblePrintingEditions = printingEditions.Distinct().AsQueryable();
            var subquery = new List<PrintingEdition>().AsQueryable();
            foreach (var type in filter.Types)
            {
                subquery = subquery.Concat(queryblePrintingEditions.Where(pe => pe.Type == type));
            }
            queryblePrintingEditions = subquery;
            if (filter.MaxPrice > filter.MinPrice && filter.MaxPrice != filter.MinPrice)
            {
                queryblePrintingEditions = queryblePrintingEditions.Where(pe => pe.Price <= filter.MaxPrice && pe.Price >= filter.MinPrice);
            }
            queryblePrintingEditions = queryblePrintingEditions
                .Skip(filter.Paging.CurrentPage * filter.Paging.ItemsCount)
                .Take(filter.Paging.ItemsCount)
                .OrderBy("Price", $"{filter.SortType}");
            printingEditions = queryblePrintingEditions.ToList();
            query = $"SELECT COUNT(*) FROM {tableName} WHERE IsRemoved = 0";
            var totalCount = await _dbContext.QueryFirstOrDefaultAsync<int>(query);
            var result = new PrintingEditionResponseDataModel
            {
                PrintingEditions = printingEditions,
                TotalCount = totalCount
            };
            return result;
        }

        public override async Task<PrintingEdition> CreateAsync(PrintingEdition model)
        {
            var query = $"INSERT INTO {tableName} " +
                $"(Id, Title, Description, Price, Currency, Type, IsRemoved, CreationDate) " +
                $"OUTPUT INSERTED.Id " +
                $"VALUES ('{model.Id}', '{model.Title}', '{model.Description}', " +
                $"{model.Price}, {(int)model.Currency}, {(int)model.Type}, 0, '{model.CreationDate.ToUniversalTime().ToString("yyyyMMdd")}' )";

            model.Id = await _dbContext.QueryFirstOrDefaultAsync<Guid>(query);
            return model;
        }

        public override async Task<PrintingEdition> UpdateAsync(PrintingEdition model)
        {
            var query = $"UPDATE {tableName} SET Title = '{model.Title}', Price = '{model.Price}'," +
                $"Type = {(int)model.Type}, Description = '{model.Description}', Currency = {(int)model.Currency} " +
                $"WHERE Id = '{model.Id}'";
            var result = await _dbContext.QueryFirstOrDefaultAsync<PrintingEdition>(query);
            return result;
        }
    }
}
