using Store.BuisnessLogic.Models.Token;
using Store.BuisnessLogic.Models.Users;
using System.Threading.Tasks;

namespace Store.BuisnessLogic.Helpers.Interfaces
{
    public interface IJwtProvider
    {
        Task SetTokenAsync(UserModel userModel);
        JwtTokenModel RefreshToken(JwtTokenModel refreshTokenModel);
    }
}