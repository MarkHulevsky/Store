﻿using Microsoft.AspNetCore.Mvc;
using Store.BuisnessLogic.Helpers;
using Store.BuisnessLogic.Helpers.Interfaces;
using Store.BuisnessLogic.Models.Account;
using Store.BuisnessLogic.Models.Token;
using Store.BuisnessLogic.Models.Users;
using Store.BuisnessLogic.Services.Interfaces;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Presentation.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IJwtProvider _jwtHelper;

        public AccountController(IAccountService accountService, IJwtProvider jwtHelper)
        {
            _accountService = accountService;
            _jwtHelper = jwtHelper;
        }

        private void SetCookieTokenResponse(string accessToken, string refreshToken)
        {
            Response.Cookies.Append("accessToken", accessToken);
            Response.Cookies.Append("refreshToken", refreshToken);
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            var token = await _accountService.GetForgotPasswordTokenAsync(model.Email);
            string newPassword = PasswordGenerator.GeneratePassword();
            var baseModel = await _accountService.ResetPasswordAsync(model.Email, token, newPassword);
            return Ok(baseModel);
        }

        [HttpPost]
        public IActionResult RefreshToken([FromBody] JwtTokenModel refreshTokenModel)
        {
            var jwtToken = new JwtTokenModel();
            var principal = _jwtHelper.GetPrincipalFromExpiredToken(refreshTokenModel.AccessToken);
            jwtToken.AccessToken = _jwtHelper.GenerateJwtTokenWithClaims(principal.Claims);
            jwtToken.RefreshToken = _jwtHelper.GenerateRefreshToken();
            SetCookieTokenResponse(jwtToken.AccessToken, jwtToken.RefreshToken);
            return Ok(jwtToken);
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] RegisterModel model)
        {
            var userModel = new UserModel();
            if (ModelState.IsValid)
            {
                var _userModelMapper = new Mapper<RegisterModel, UserModel>();
                userModel = _userModelMapper.Map(model);

                var result = await _accountService.RegisterAsync(userModel);
                if (result.Succeeded)
                {
                    var token = await _accountService.GenerateEmailConfirmationTokenAsync(model.Email);
                    string url = Url.Action("ConfirmEmail", "Account",
                        new { email = model.Email, token }, Request.Scheme);
                    await _accountService.SendConfirmUrlAsync(model.Email, url);
                    return Ok();
                }
                foreach (var error in result.Errors)
                {
                    userModel.Errors.Add(error.Description);
                }
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                userModel.Errors.Add(error.ErrorMessage);
            }

            return Ok(userModel);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            var result = await _accountService.ConfirmEmail(email, token);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody] LoginModel model)
        {
            var userModel = new UserModel
            {
                Email = model.Email,
                Password = model.Password
            };
            var result = await _accountService.LoginAsync(userModel);

            if (result.Errors.Count != 0)
            {
                return Unauthorized(userModel);
            }
            userModel = await _accountService.FindByEmailAsync(userModel.Email);
            userModel.Roles = await _accountService.GetRolesAsync(userModel.Email);
            var jwtTokenModel = new JwtTokenModel
            {
                AccessToken = await _jwtHelper.GetTokenAsync(userModel),
                RefreshToken = _jwtHelper.GenerateRefreshToken()
            };
            SetCookieTokenResponse(jwtTokenModel.AccessToken, jwtTokenModel.RefreshToken);
            return Ok(userModel);
        }

        [HttpPost]
        public Task SignOut()
        {
            return _accountService.LogoutAsync();
        }
    }
}
