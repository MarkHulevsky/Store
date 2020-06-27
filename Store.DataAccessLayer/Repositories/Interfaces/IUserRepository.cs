using Microsoft.AspNetCore.Identity;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Repositories.Interfaces
{
    public interface IUserRepository: IRepository<User>
    {
        List<User> Filter(UserRequestFilter filter);
        Task<IdentityResult> ResetPasswordAsync(string email, string token, string newPassword);
        Task<string> GetFrogotPasswordTokenAsync(string email);
        new Task<IdentityResult> CreateAsync(User user);
        Task<User> FindByEmailAsync(string email);
        Task<IdentityResult> AddToRoleAsync(User user ,string roleName);
        Task<string> GetRoleNameAsync(User user);
        Task<SignInResult> SignInAsync(User user);
        Task SignOutAsync();

    }
}
