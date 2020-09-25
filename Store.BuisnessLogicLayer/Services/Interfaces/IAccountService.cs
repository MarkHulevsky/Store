using Microsoft.AspNetCore.Identity;
using Store.BuisnessLogic.Models.Account;
using Store.BuisnessLogic.Models.Base;
using Store.BuisnessLogic.Models.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.BuisnessLogic.Services.Interfaces
{
    public interface IAccountService
    {
        Task<BaseModel> ResetPasswordAsync(string email);
        Task<List<string>> GetRolesAsync(string email);
        Task<UserModel> FindByEmailAsync(string email);
        Task<UserModel> RegisterAsync(RegisterModel userModel);
        Task<BaseModel> ConfirmEmail(string userEmail, string token);
        Task<UserModel> LoginAsync(LoginModel userModel);
        Task LogoutAsync();
    }
}
