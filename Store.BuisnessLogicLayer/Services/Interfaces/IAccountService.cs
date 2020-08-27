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
        Task<IdentityResult> RegisterAsync(UserModel user);
        Task SendConfirmUrlAsync(string email, string url);
        Task ConfirmEmail(string userEmail);
        Task<BaseModel> LoginAsync(UserModel user);
        Task LogoutAsync();
    }
}
