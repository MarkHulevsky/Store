using Store.DataAccess.Entities;
using Store.DataAccess.Filters;
using Store.DataAccess.Filters.ResponseFulters;
using System.Threading.Tasks;

namespace Store.DataAccess.Repositories.Interfaces
{
    public interface IAuthorRepository : IRepository<Author>
    {
        Task<AuthorResponseDataModel> FilterAsync(AuthorRequestDataModel authorRequestDataModel);
        Task<Author> FindAuthorByNameAsync(string name);
    }
}
