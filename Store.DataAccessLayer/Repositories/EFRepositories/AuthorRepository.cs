using Microsoft.EntityFrameworkCore;
using Store.DataAccessLayer.AppContext;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Repositories.Base;
using Store.DataAccessLayer.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Store.DataAccessLayer.Filters;

namespace Store.DataAccessLayer.Repositories.EFRepositories
{
    public class AuthorRepository : BaseEFRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<List<Author>> FilterAsync(AuthorRequestFilter filter)
        {
            var query = _dbContext.Authors.Where(a => !a.IsRemoved)
                .OrderBy(filter.PropName, filter.SortType.ToString());

            return await query.Skip(filter.Paging.ItemsCount * filter.Paging.Number)
                .Take(filter.Paging.ItemsCount).ToListAsync();
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
            return await _dbContext.AuthorInPrintingEditions.Select(ap => ap.PrintingEdition).ToListAsync();
        }
    }
}
