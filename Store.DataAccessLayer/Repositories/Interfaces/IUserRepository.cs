using Microsoft.AspNetCore.Identity;
using Store.DataAccess.Filters;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Repositories.Interfaces
{
    public interface IUserRepository: IRepository<User>
    {
        new Task<IdentityResult> UpdateAsync(User user);
        UserResponseFilter Filter(UserRequestFilter filter);
        Task<IdentityResult> ResetPasswordAsync(string email, string token, string newPassword);
        Task<string> GetFrogotPasswordTokenAsync(string email);
        new Task<IdentityResult> CreateAsync(User user);
        Task<User> FindByEmailAsync(string email);
        Task<IdentityResult> AddToRoleAsync(User user ,string roleName);
        Task<List<string>> GetRolesAsync(User user);
        Task<SignInResult> SignInAsync(User user);
        Task SignOutAsync();

    }
}
