using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Store.BuisnessLogicLayer.Models.Users;
using Store.BuisnessLogicLayer.Services.Interfaces;
using Store.Presentation.Helpers.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
namespace Store.Presentation.Helpers
{
    public class JwtHelper: IJwtHelper
    {
        private readonly IAccountService _accountService;
        private readonly IConfiguration _configuration;
        public JwtHelper(IAccountService accountService, IConfiguration configuration)
        {
            _accountService = accountService;
            _configuration = configuration;
        }
        public async Task<string> GetTokenAsync(UserModel userModel)
        {
            var user = await _accountService.FindByEmailAsync(userModel.Email);
            var roleName = await _accountService.GetRoleNameAsync(user.Email);
            
            var accessClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, roleName),
                new Claim(ClaimTypes.Name, user.Email),
            };

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(
                            _configuration.GetSection("JwtSettings")["Key"]));

            var credentials = new SigningCredentials(symmetricSecurityKey,
                SecurityAlgorithms.HmacSha256);

            var now = DateTime.Now;

            var accessToken = new JwtSecurityToken(
                issuer: _configuration.GetSection("JwtSettings")["Issuer"],
                audience: _configuration.GetSection("JwtSettings")["Audience"],
                claims: accessClaims,
                expires: now.Add(TimeSpan.FromMinutes(int.Parse(_configuration.GetSection("JwtSettings")["Lifetime"]))),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(accessToken);
        }
    }
}
