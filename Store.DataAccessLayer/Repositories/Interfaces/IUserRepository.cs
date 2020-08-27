using Store.DataAccess.Entities;
using Store.DataAccess.Filters;
using System.Threading.Tasks;

namespace Store.DataAccess.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<UserResponseDataModel> FilterAsync(UserRequestDataModel filter);
        Task<User> FindByEmailAsync(string email);
    }
}
