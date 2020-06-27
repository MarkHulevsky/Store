using Microsoft.EntityFrameworkCore;
using Store.DataAccessLayer.AppContext;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Filters;
using Store.DataAccessLayer.Repositories.Base;
using Store.DataAccessLayer.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Repositories.EFRepositories
{
    public class PrintingEditionRepository : BaseEFRepository<PrintingEdition>, IPrintingEditionRepository
    {
        public PrintingEditionRepository(ApplicationContext context)
            : base(context)
        {
        }

        public List<PrintingEdition> Filter(PrintingEditionsRequestFilter filter)
        {
            var query = _dbContext.PrintingEditions.Include(pe => pe.AuthorInPrintingEditions)
                .ThenInclude(aInPe => aInPe.Author)
                .Where(pe => !pe.IsRemoved && EF.Functions.Like(pe.Title, $"%{filter.SearchFilter}%"));

            query = query.Where(pe => pe.Currency == filter.Currency);

            var uQuery = new List<PrintingEdition>().AsQueryable();

            foreach(var type in filter.Types)
            {
                uQuery = uQuery.Concat(query.Where(pe => pe.Type == type));
            }
            query = uQuery;

            if (filter.MaxPrice > filter.MinPrice && filter.MaxPrice != filter.MinPrice)
            {
                query = query.Where(pe => pe.Price <= filter.MaxPrice && pe.Price >= filter.MinPrice);
            }

            if (filter.SortType.ToString() == "Ascending")
            {
                query.OrderBy(pe => pe.Price);
            }
            if (filter.SortType.ToString() == "Descending")
            {
                query.OrderByDescending(pe => pe.Price);
            }

             return query.Skip(filter.Paging.Number * filter.Paging.ItemsCount)
                .Take(filter.Paging.ItemsCount).ToList();
        }

        public override async Task<PrintingEdition> UpdateAsync(PrintingEdition model)
        {
            var entity = await _dbContext.PrintingEditions.FirstOrDefaultAsync(pe => pe.Id == model.Id);
            entity.Title = model.Title;
            entity.Price = model.Price;
            entity.Type = model.Type;
            entity.Description = model.Description;
            entity.Currency = entity.Currency;
            entity = _dbContext.PrintingEditions.Update(entity).Entity;
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<PrintingEdition> FindByTitleAsync(string title)
        {
            return await _dbContext.PrintingEditions.FirstOrDefaultAsync(pe => pe.Title == title);
        }

        public async Task<List<Author>> GetAuthorsAsync(PrintingEdition printingEdition)
        {
            var pe = await _dbContext.PrintingEditions.Include(pe => pe.AuthorInPrintingEditions)
                .ThenInclude(ae => ae.Author)
                .FirstOrDefaultAsync(pe => pe.Id == printingEdition.Id);

            return pe.AuthorInPrintingEditions.Select(ap => ap.Author).ToList();
        }
    }
}
