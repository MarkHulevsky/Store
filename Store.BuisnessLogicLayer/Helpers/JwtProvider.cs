﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Store.BuisnessLogic.Helpers.Interfaces;
using Store.BuisnessLogic.Models.Token;
using Store.BuisnessLogic.Models.Users;
using Store.BuisnessLogic.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Store.BuisnessLogic.Helpers
{
    public class JwtProvider : IJwtProvider
    {
        private const string ACCESS_TOKEN_NAME = "accessToken";
        private const string REFRESH_TOKEN_NAME = "refreshToken";

        private readonly IAccountService _accountService;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtProvider(IAccountService accountService, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _accountService = accountService;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetCookieTokenResponse(JwtTokenModel jwtToken)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Append(ACCESS_TOKEN_NAME, jwtToken.AccessToken);
            _httpContextAccessor.HttpContext.Response.Cookies.Append(REFRESH_TOKEN_NAME, jwtToken.RefreshToken);
        }

        public string GenerateRefreshToken()
        {
            string refreshToken = string.Empty;
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                refreshToken = Convert.ToBase64String(randomNumber);
            }
            return refreshToken;
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    _configuration.GetSection("JwtSettings")["Key"])),
                ValidateLifetime = false,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            IdentityModelEventSource.ShowPII = true;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        public async Task<string> GetTokenAsync(UserModel userModel)
        {
            var user = await _accountService.FindByEmailAsync(userModel.Email);

            var accessClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, userModel.Roles[0]),
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

        public string GenerateJwtTokenWithClaims(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JwtSettings")["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now
                .Add(TimeSpan.FromMinutes(int.Parse(_configuration.GetSection("JwtSettings")["Lifetime"])));
            var jwt = new JwtSecurityToken(
                _configuration.GetSection("JwtSettings")["Issuer"],
                _configuration.GetSection("JwtSettings")["Issuer"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
