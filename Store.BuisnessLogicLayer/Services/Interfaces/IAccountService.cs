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
        Task<BaseModel> RegisterAsync(RegisterModel userModel);
        Task SendConfirmUrlAsync(string email);
        Task<BaseModel> ConfirmEmail(string userEmail, string token);
        Task<BaseModel> LoginAsync(LoginModel userModel);
        Task LogoutAsync();
    }
}
