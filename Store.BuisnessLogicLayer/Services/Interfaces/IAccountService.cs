using Microsoft.AspNetCore.Identity;
using Store.BuisnessLogic.Models.Base;
using Store.BuisnessLogic.Models.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.BuisnessLogic.Services.Interfaces
{
    public interface IAccountService
    {
        Task<BaseModel> ResetPasswordAsync(string email, string token, string newPassword);
        Task<string> GetForgotPasswordTokenAsync(string email);
        Task<List<string>> GetRolesAsync(string email);
        Task<UserModel> FindByEmailAsync(string email);
        Task<string> GenerateEmailConfirmationTokenAsync(string email);
        Task<IdentityResult> RegisterAsync(UserModel userModel);
        Task SendConfirmUrlAsync(string email, string url);
        Task<BaseModel> ConfirmEmail(string userEmail, string token);
        Task<BaseModel> LoginAsync(UserModel userModel);
        Task LogoutAsync();
    }
}
