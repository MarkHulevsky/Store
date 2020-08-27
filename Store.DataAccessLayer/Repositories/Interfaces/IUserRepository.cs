using Store.DataAccess.Entities;
using Store.DataAccess.Filters;
using System.Threading.Tasks;

namespace Store.DataAccess.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        UserResponseDataModel Filter(UserRequestDataModel filter);
        Task<User> FindByEmailAsync(string email);
    }
}
