using Store.BuisnessLogic.Models.Token;
using Store.BuisnessLogic.Models.Users;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Store.BuisnessLogic.Helpers.Interfaces
{
    public interface IJwtProvider
    {
        void SetCookieTokenResponse(JwtTokenModel jwtToken);
        Task<string> GetTokenAsync(UserModel userModel);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        string GenerateJwtTokenWithClaims(IEnumerable<Claim> claims);
    }
}