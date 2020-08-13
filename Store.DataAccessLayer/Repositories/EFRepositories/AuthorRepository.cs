using Microsoft.EntityFrameworkCore;
using Store.DataAccess.Filters.ResponseFulters;
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
    public class AuthorRepository : BaseEFRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<AuthorResponseFilter> FilterAsync(AuthorRequestFilter filter)
        {
            var query = _dbContext.Authors.Where(a => !a.IsRemoved)
                .OrderBy(filter.PropName, filter.SortType.ToString());

            var authors = await query.Skip(filter.Paging.ItemsCount * filter.Paging.CurrentPage)
                .Take(filter.Paging.ItemsCount).ToListAsync();

            foreach (var author in authors)
            {
                author.PrintingEditions = await GetPrintingEditionsAsync(author);
            }

            var result = new AuthorResponseFilter
            {
                Authors = authors,
                TotalCount = _dbContext.Authors.Where(a => !a.IsRemoved).Count()
            };

            return result;
        }

        public override async Task<Author> UpdateAsync(Author author)
        {
            var entity = await _dbContext.Authors.FirstOrDefaultAsync(a => a.Id == author.Id);
            entity.Name = author.Name;
            entity = _dbContext.Authors.Update(entity).Entity;
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<Author> FindAuthorByNameAsync(string name)
        {
            return await _dbContext.Authors.FirstOrDefaultAsync(a => a.Name == name);
        }

        public async Task<List<PrintingEdition>> GetPrintingEditionsAsync(Author author)
        {
            var authorInPrintingEditions = await _dbContext.AuthorInPrintingEditions
                .Where(aInPe => aInPe.AuthorId == author.Id).ToListAsync();
            var printingEditions = new List<PrintingEdition>();
            foreach (var aInPe in authorInPrintingEditions)
            {
                var printingEdition = _dbContext.PrintingEditions.FirstOrDefault(pe => pe.Id == aInPe.PrintingEditionId
                && !pe.IsRemoved);
                if (printingEdition != null)
                {
                    printingEditions.Add(printingEdition);
                }
            }
            return printingEditions;
        }
    }
}
