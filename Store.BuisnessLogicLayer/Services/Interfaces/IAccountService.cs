using Store.BuisnessLogic.Models.Account;
using Store.BuisnessLogic.Models.Base;
using System.Threading.Tasks;

namespace Store.BuisnessLogic.Services.Interfaces
{
    public interface IAccountService
    {
        Task<BaseModel> ResetPasswordAsync(string email);
        Task<BaseModel> RegisterAsync(RegisterModel userModel);
        Task<BaseModel> ConfirmEmail(string userEmail, string token);
        Task<BaseModel> LoginAsync(LoginModel userModel);
        Task LogoutAsync();
    }
}
