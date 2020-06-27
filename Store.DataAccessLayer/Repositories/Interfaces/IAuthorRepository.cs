using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Repositories.Interfaces
{
    public interface IAuthorRepository: IRepository<Author>
    {
        Task<List<Author>> FilterAsync(AuthorRequestFilter filter);
        Task<Author> FindAuthorByNameAsync(string name);
        Task<List<PrintingEdition>> GetPrintingEditionsAsync(Author author);
    }
}
