using Store.BuisnessLogic.Models.Token;
using Store.BuisnessLogic.Models.Users;

namespace Store.BuisnessLogic.Helpers.Interfaces
{
    public interface IJwtProvider
    {
        void SetToken(UserModel userModel);
        JwtTokenModel RefreshToken(JwtTokenModel refreshTokenModel);
    }
}