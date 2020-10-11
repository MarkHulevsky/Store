﻿using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Store.DataAccess.Entities;
using Store.DataAccess.Filters;
using Store.DataAccess.Filters.ResponseFulters;
using Store.DataAccess.Models.Constants;
using Store.DataAccess.Repositories.Base;
using Store.DataAccess.Repositories.Interfaces;
using System.Linq;
using System.Text;
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
            var query = new StringBuilder();
            query.Append($@"SELECT PrintingEditions.Id, PrintingEditions.Currency, PrintingEditions.Description, PrintingEditions.IsRemoved,
	                        PrintingEditions.Price, PrintingEditions.CreationDate, PrintingEditions.Title, PrintingEditions.Type,
	                        AuthorInPrintingEditions.Id, AuthorInPrintingEditions.AuthorId, AuthorInPrintingEditions.PrintingEditionId,
	                        AuthorInPrintingEditions.AuthorId, AuthorInPrintingEditions.Name, AuthorInPrintingEditions.IsRemoved
	                        FROM (
	                        	SELECT PrintingEditions.Id, PrintingEditions.Currency, PrintingEditions.Description, PrintingEditions.IsRemoved,
	                        		PrintingEditions.Price, PrintingEditions.CreationDate, PrintingEditions.Title, PrintingEditions.Type
	                        	FROM PrintingEditions WHERE (PrintingEditions.Title LIKE '%{printingEditionRequestDataModel.SearchString}%'
                                    AND PrintingEditions.IsRemoved != 1) ");

            if (printingEditionRequestDataModel.MaxPrice > printingEditionRequestDataModel.MinPrice
                    && printingEditionRequestDataModel.MaxPrice != printingEditionRequestDataModel.MinPrice)
            {
                query.Append($@"AND (PrintingEditions.Price < {printingEditionRequestDataModel.MaxPrice} 
                                AND PrintingEditions.Price > {printingEditionRequestDataModel.MinPrice}) ");

            }
            if (printingEditionRequestDataModel.Types.Any())
            {
                query.Append("AND (");
            }
            foreach (var type in printingEditionRequestDataModel.Types)
            {
                query.Append($"PrintingEditions.Type = {(int)type} OR ");
            }
            if (printingEditionRequestDataModel.Types.Any())
            {
                var index = query.ToString().LastIndexOf("OR");
                var removeLength = query.Length - index;
                query.Remove(index, removeLength);
                query.Append(")");
            }
            query.Append($@") AS PrintingEditions
	                        LEFT JOIN (
	                        	 SELECT AuthorInPrintingEditions.AuthorId, AuthorInPrintingEditions.PrintingEditionId,
	                        	 Authors.Id, Authors.Name, Authors.IsRemoved
	                        	 FROM AuthorInPrintingEditions
	                        	 LEFT JOIN Authors ON AuthorInPrintingEditions.AuthorId = Authors.Id
	                        ) AS AuthorInPrintingEditions ON PrintingEditions.Id = AuthorInPrintingEditions.PrintingEditionId ");
            query.Append($@"ORDER BY Price {printingEditionRequestDataModel.SortType.ToString().ToUpper()}
	                        OFFSET {printingEditionRequestDataModel.Paging.ItemsCount * printingEditionRequestDataModel.Paging.CurrentPage} ROWS
                            FETCH NEXT {printingEditionRequestDataModel.Paging.ItemsCount} ROWS ONLY");
            using (var dbContext = new SqlConnection(connectionString))
            {
                await dbContext.OpenAsync();
                var printingEditions = await dbContext.QueryAsync<PrintingEdition, Author, PrintingEdition>(
                query.ToString(), (printingEdition, author) =>
                {
                    if (author != null)
                    {
                        printingEdition.Authors.Add(author);
                    }
                    return printingEdition;
                });
                printingEditions = printingEditions
                    .GroupBy(printingEdition => printingEdition.Id)
                    .Select(group =>
                    {
                        var result = group.FirstOrDefault();
                        result.Authors = group.Select(printingEdition => printingEdition.Authors.SingleOrDefault()).ToList();
                        return result;
                    });
                query.Clear();
                query.Append(@$"SELECT COUNT(*) FROM {tableName} WHERE {tableName}.Title LIKE '%{printingEditionRequestDataModel.SearchString}%'
                                AND {tableName}.IsRemoved = 0");
                var totalCount = await dbContext.QueryFirstOrDefaultAsync<int>(query.ToString());
                var result = new PrintingEditionResponseDataModel
                {
                    PrintingEditions = printingEditions,
                    TotalCount = totalCount
                };
                return result;
            }
        }
    }
}
