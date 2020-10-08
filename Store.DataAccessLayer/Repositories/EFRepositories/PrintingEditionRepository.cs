using Microsoft.EntityFrameworkCore;
using Store.DataAccess.AppContext;
using Store.DataAccess.Entities;
using Store.DataAccess.Filters;
using Store.DataAccess.Filters.ResponseFulters;
using Store.DataAccess.Repositories.Base;
using Store.DataAccess.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.DataAccess.Repositories.EFRepositories
{
    public class PrintingEditionRepository : BaseEFRepository<PrintingEdition>, IPrintingEditionRepository
    {
        public PrintingEditionRepository(ApplicationContext context)
            : base(context)
        {
        }

        public async Task<PrintingEditionResponseDataModel> FilterAsync(PrintingEditionsRequestDataModel printingEditionRequestDataModel)
        {
            var query = DbSet
                .Include(printingEdition => printingEdition.AuthorInPrintingEditions)
                .ThenInclude(authorInPrintingEdition => authorInPrintingEdition.Author)
                .Where(pe => !pe.IsRemoved && EF.Functions.Like(pe.Title, $"%{printingEditionRequestDataModel.SearchString}%"));
            var totalCount = await query.CountAsync();
            var subquery = new List<PrintingEdition>().AsQueryable();
            foreach (var type in printingEditionRequestDataModel.Types)
            {
                subquery = subquery.Concat(query.Where(pe => pe.Type == type));
            }
            query = subquery;

            if (printingEditionRequestDataModel.MaxPrice > printingEditionRequestDataModel.MinPrice
                    && printingEditionRequestDataModel.MaxPrice != printingEditionRequestDataModel.MinPrice)
            {
                query = query.Where(pe => pe.Price <= printingEditionRequestDataModel.MaxPrice
                    && pe.Price >= printingEditionRequestDataModel.MinPrice);
            }
            query = query
                .OrderBy("Price", $"{printingEditionRequestDataModel.SortType}")
                .Skip(printingEditionRequestDataModel.Paging.CurrentPage * printingEditionRequestDataModel.Paging.ItemsCount)
                .Take(printingEditionRequestDataModel.Paging.ItemsCount);
            foreach (var printingEdition in query)
            {
                var authors = printingEdition.AuthorInPrintingEditions
                    .Select(authorInPrintingEditions => authorInPrintingEditions.Author)
                    .ToList();
                printingEdition.Authors = authors;
            }
            var printingEditions = query.ToList();
            var result = new PrintingEditionResponseDataModel
            {
                PrintingEditions = printingEditions,
                TotalCount = totalCount
            };
            return result;
        }

        public override async Task<PrintingEdition> UpdateAsync(PrintingEdition model)
        {
            var entity = await DbSet.FirstOrDefaultAsync(pe => pe.Id == model.Id);
            entity.Title = model.Title;
            entity.Price = model.Price;
            entity.Type = model.Type;
            entity.Description = model.Description;
            entity.Currency = entity.Currency;
            DbSet.Update(entity);
            await SaveChangesAsync();
            return entity;
        }
    }
}
