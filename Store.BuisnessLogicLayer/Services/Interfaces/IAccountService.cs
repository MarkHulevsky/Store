using Store.BuisnessLogic.Models.Account;
using Store.BuisnessLogic.Models.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.BuisnessLogic.Services.Interfaces
{
    public interface IAccountService
    {
        Task<List<string>> ResetPasswordAsync(string email);
        Task<List<string>> RegisterAsync(RegisterModel userModel);
        Task<List<string>> ConfirmEmail(string userEmail, string token);
        Task<UserModel> LoginAsync(LoginModel userModel);
        Task LogoutAsync();
    }
}
