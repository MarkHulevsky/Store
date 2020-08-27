using Store.BuisnessLogic.Models.Users;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Store.BuisnessLogic.Helpers.Interfaces
{
    public interface IJwtHelper
    {
        Task<string> GetTokenAsync(UserModel userModel);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        string GenerateJwtTokenWithClaims(IEnumerable<Claim> claims);
    }
}