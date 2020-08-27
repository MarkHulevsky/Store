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

        public PrintingEditionResponseDataModel Filter(PrintingEditionsRequestDataModel filter)
        {
            var query = DbSet
                .Include(printingEdition => printingEdition.AuthorInPrintingEditions)
                .ThenInclude(authorInPrintingEdition => authorInPrintingEdition.Author)
                .Where(pe => !pe.IsRemoved && EF.Functions.Like(pe.Title, $"%{filter.SearchString}%"));

            var subquery = new List<PrintingEdition>().AsQueryable();

            foreach (var type in filter.Types)
            {
                subquery = subquery.Concat(query.Where(pe => pe.Type == type));
            }

            query = subquery;

            if (filter.MaxPrice > filter.MinPrice && filter.MaxPrice != filter.MinPrice)
            {
                query = query.Where(pe => pe.Price <= filter.MaxPrice && pe.Price >= filter.MinPrice);
            }

            query = query.OrderBy("Price", $"{filter.SortType}");
            var printingEditions = query.Skip(filter.Paging.CurrentPage * filter.Paging.ItemsCount)
                .Take(filter.Paging.ItemsCount).ToList();
            foreach (var printingEdition in printingEditions)
            {
                var authors = printingEdition.AuthorInPrintingEditions
                    .Select(authorInPrintingEditions => authorInPrintingEditions.Author)
                    .ToList();
                printingEdition.Authors = authors;
            }
            var result = new PrintingEditionResponseDataModel
            {
                PrintingEditions = printingEditions,
                TotalCount = DbSet.Where(pe => !pe.IsRemoved).Count(),
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
            entity = DbSet.Update(entity).Entity;
            await SaveChangesAsync();
            return entity;
        }
    }
}
