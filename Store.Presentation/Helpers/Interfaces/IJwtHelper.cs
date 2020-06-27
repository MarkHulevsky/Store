using Store.BuisnessLogicLayer.Models.Users;
using System.Threading.Tasks;

namespace Store.Presentation.Helpers.Interfaces
{
    public interface IJwtHelper
    {
        Task<string> GetTokenAsync(UserModel userModel);
    }
}
