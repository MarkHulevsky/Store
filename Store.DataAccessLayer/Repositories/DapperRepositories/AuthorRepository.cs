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
using System.Text;
using System.Threading.Tasks;
using static Shared.Enums.Enums;

namespace Store.DataAccess.Repositories.DapperRepositories
{
    public class AuthorRepository : BaseDapperRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(IConfiguration configuration) : base(configuration)
        {
            tableName = Constants.AUTHORS_TABLE_NAME;
        }

        public async Task<AuthorResponseDataModel> FilterAsync(AuthorRequestDataModel authorRequestDataModel)
        {
            var sortTypeString = string.Empty;
            if (authorRequestDataModel.SortType == SortType.Ascending)
            {
                sortTypeString = "ASC";
            }
            if (authorRequestDataModel.SortType == SortType.Descending)
            {
                sortTypeString = "DESC";
            }

            var query = $@"SELECT t.Id, t.Name, t.IsRemoved, authorInPrintingEdition.Id,
	                        authorInPrintingEdition.AuthorId, authorInPrintingEdition.PrintingEditionId,
	                        authorInPrintingEdition.aInPEId, authorInPrintingEdition.Currency,
	                        authorInPrintingEdition.Description, authorInPrintingEdition.IsRemoved,
	                        authorInPrintingEdition.Price, authorInPrintingEdition.CreationDate,
	                        authorInPrintingEdition.Title, authorInPrintingEdition.Type
	                          FROM (
	                        	  SELECT a.Id, a.IsRemoved, a.Name
	                        	  FROM {tableName} AS a
	                        	  WHERE a.IsRemoved != 1
	                        	  ORDER BY a.Id {sortTypeString}
	                        	  OFFSET {authorRequestDataModel.Paging.CurrentPage * authorRequestDataModel.Paging.ItemsCount} ROWS 
                                  FETCH NEXT {authorRequestDataModel.Paging.ItemsCount} ROWS ONLY
	                          ) AS t
	                          LEFT JOIN (
	                        	  SELECT aInPe.Id, aInPe.AuthorId, aInPe.PrintingEditionId, p.Id AS aInPeId, p.Currency,
	                        		p.Description, p.IsRemoved, p.Price, p.CreationDate, p.Title, p.Type
	                        	  FROM {Constants.AUTHOR_IN_PRINTING_EDITIONS_TABLE_NAME} AS aInPe
	                        	  LEFT JOIN {Constants.PRINTING_EDITIONS_TABLE_NAME} AS p ON aInPe.PrintingEditionId = p.Id
	                          ) AS authorInPrintingEdition ON t.Id = authorInPrintingEdition.AuthorId
	                        ORDER BY t.Id {sortTypeString}, authorInPrintingEdition.Id";

            var authorsDictionary = new Dictionary<Guid, Author>();
            var authors = await _dbContext.QueryAsync<Author, AuthorInPrintingEdition, PrintingEdition, Author>(
                query, (author, authorInPrintingEdition, printingEdition) =>
                {
                    var authorEntry = new Author();
                    if (!authorsDictionary.TryGetValue(author.Id, out authorEntry))
                    {
                        authorEntry = author;
                        authorsDictionary.Add(authorEntry.Id, authorEntry);
                    }
                    if (printingEdition != null)
                    {
                        authorEntry.PrintingEditions.Add(printingEdition);
                    }
                    return authorEntry;
                },
                splitOn: "Id, Id, aInPeId");
            authors = authors.Distinct();
            query = $"SELECT COUNT(*) FROM {tableName} WHERE IsRemoved != 1";
            var totalCount = await _dbContext.QueryFirstOrDefaultAsync<int>(query.ToString());
            var result = new AuthorResponseDataModel
            {
                Authors = authors.ToList(),
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
    }
}
