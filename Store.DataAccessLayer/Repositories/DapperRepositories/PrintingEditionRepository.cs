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

        public async Task<PrintingEditionResponseDataModel> FilterAsync(PrintingEditionsRequestDataModel printingEditionRequestDataModel)
        {
            var query = $"SELECT * FROM {tableName} LEFT JOIN (" +
                $"SELECT {Constants.AUTHOR_IN_PRINTING_EDITIONS_TABLE_NAME}.PrintingEditionId, " +
                $"{Constants.AUTHOR_IN_PRINTING_EDITIONS_TABLE_NAME}.AuthorId FROM {Constants.AUTHOR_IN_PRINTING_EDITIONS_TABLE_NAME} " +
                $") AS {Constants.AUTHOR_IN_PRINTING_EDITIONS_TABLE_NAME} " +
                $"ON {Constants.AUTHOR_IN_PRINTING_EDITIONS_TABLE_NAME}.PrintingEditionId = {tableName}.Id " +
                $"LEFT JOIN {Constants.AUTHORS_TABLE_NAME} " +
                $"ON {Constants.AUTHOR_IN_PRINTING_EDITIONS_TABLE_NAME}.AuthorId = {Constants.AUTHORS_TABLE_NAME}.Id " +
                $"WHERE {tableName}.Title LIKE '%{printingEditionRequestDataModel.SearchString}%' AND {tableName}.IsRemoved = 0";

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
            foreach (var type in printingEditionRequestDataModel.Types)
            {
                subquery = subquery.Concat(queryblePrintingEditions.Where(pe => pe.Type == type));
            }
            queryblePrintingEditions = subquery;
            if (printingEditionRequestDataModel.MaxPrice > printingEditionRequestDataModel.MinPrice
                    && printingEditionRequestDataModel.MaxPrice != printingEditionRequestDataModel.MinPrice)
            {
                queryblePrintingEditions = queryblePrintingEditions.Where(pe => pe.Price <= printingEditionRequestDataModel.MaxPrice
                    && pe.Price >= printingEditionRequestDataModel.MinPrice);
            }
            queryblePrintingEditions = queryblePrintingEditions
                .OrderBy("Price", $"{printingEditionRequestDataModel.SortType}")
                .Skip(printingEditionRequestDataModel.Paging.CurrentPage * printingEditionRequestDataModel.Paging.ItemsCount)
                .Take(printingEditionRequestDataModel.Paging.ItemsCount);
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

        public override async Task<PrintingEdition> CreateAsync(PrintingEdition printingEdition)
        {
            var query = $"INSERT INTO {tableName} " +
                $"(Id, Title, Description, Price, Currency, Type, IsRemoved, CreationDate) " +
                $"OUTPUT INSERTED.Id " +
                $"VALUES ('{Guid.NewGuid()}', '{printingEdition.Title}', '{printingEdition.Description}', " +
                $"{printingEdition.Price}, {(int)printingEdition.Currency}, {(int)printingEdition.Type}, 0, '{printingEdition.CreationDate.ToUniversalTime():yyyyMMdd}' )";

            printingEdition.Id = await _dbContext.QueryFirstOrDefaultAsync<Guid>(query);
            return printingEdition;
        }

        public override async Task<PrintingEdition> UpdateAsync(PrintingEdition printingEdition)
        {
            var query = $"UPDATE {tableName} SET Title = '{printingEdition.Title}', Price = '{printingEdition.Price}'," +
                $"Type = {(int)printingEdition.Type}, Description = '{printingEdition.Description}', Currency = {(int)printingEdition.Currency} " +
                $"WHERE Id = '{printingEdition.Id}'";
            var result = await _dbContext.QueryFirstOrDefaultAsync<PrintingEdition>(query);
            return result;
        }
    }
}
