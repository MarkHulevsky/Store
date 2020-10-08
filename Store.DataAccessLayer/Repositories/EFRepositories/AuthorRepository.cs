using Microsoft.EntityFrameworkCore;
using Store.DataAccess.AppContext;
using Store.DataAccess.Entities;
using Store.DataAccess.Filters;
using Store.DataAccess.Filters.ResponseFulters;
using Store.DataAccess.Repositories.Base;
using Store.DataAccess.Repositories.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Store.DataAccess.Repositories.EFRepositories
{
    public class AuthorRepository : BaseEFRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<AuthorResponseDataModel> FilterAsync(AuthorRequestDataModel authorRequestDataModel)
        {
            var queryableAuthors = DbSet.Where(a => !a.IsRemoved)
                .Include(author => author.AuthorInPrintingEditions)
                .ThenInclude(authorInPrintingEdition => authorInPrintingEdition.PrintingEdition)
                .OrderBy(authorRequestDataModel.SortPropertyName, authorRequestDataModel.SortType.ToString());
            var totalCount = await queryableAuthors.CountAsync();
            var authors = await queryableAuthors
                .Skip(authorRequestDataModel.Paging.ItemsCount * authorRequestDataModel.Paging.CurrentPage)
                .Take(authorRequestDataModel.Paging.ItemsCount)
                .ToListAsync();
            foreach (var author in authors)
            {
                var printingEditions = author.AuthorInPrintingEditions
                    .Select(authorInPrintingEdition => authorInPrintingEdition.PrintingEdition)
                    .ToList();
                author.PrintingEditions = printingEditions;
            }
            var result = new AuthorResponseDataModel
            {
                Authors = authors,
                TotalCount = totalCount
            };
            return result;
        }

        public override async Task<Author> UpdateAsync(Author author)
        {
            var entity = await DbSet.FirstOrDefaultAsync(a => a.Id == author.Id);
            entity.Name = author.Name;
            entity = DbSet.Update(entity).Entity;
            await SaveChangesAsync();
            return entity;
        }

        public async Task<Author> FindAuthorByNameAsync(string name)
        {
            var author = await DbSet.FirstOrDefaultAsync(a => a.Name == name);
            return author;
        }
    }
}
