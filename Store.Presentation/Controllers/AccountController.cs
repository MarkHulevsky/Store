using Microsoft.AspNetCore.Mvc;
using Store.BuisnessLogic.Helpers;
using Store.BuisnessLogic.Models.Token;
using Store.BuisnessLogicLayer.Helpers;
using Store.BuisnessLogicLayer.Models.Account;
using Store.BuisnessLogicLayer.Models.Users;
using Store.BuisnessLogicLayer.Services.Interfaces;
using Store.Presentation.Helpers.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Presentation.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IJwtHelper _jwtHelper;

        public AccountController(IAccountService accountService, IJwtHelper jwtHelper)
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
            var userModel = await _accountService.FindByEmailAsync(model.Email);
            if (userModel != null)
            {
                var token = await _accountService.GetForgotPasswordTokenAsync(model.Email);
                string newPassword = PasswordGenerator.GeneratePassword();

                var result = await _accountService.ResetPasswordAsync(model.Email, token, newPassword);

                if (result.Errors.Count > 0)
                {
                    userModel.Errors = result.Errors;
                    return Ok(userModel);
                }

                return Ok();
            }
            userModel = new UserModel();
            userModel.Errors.Add("No user with such email");
            return Ok(userModel);
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
                var userModelMapper = new Mapper<RegisterModel, UserModel>();
                userModel = userModelMapper.Map(new UserModel(), model);

                var result = await _accountService.RegisterAsync(userModel);
                if (result.Succeeded)
                {
                    string url = Url.Action("ConfirmEmail", "Account",
                        new { email = model.Email }, Request.Scheme);
                    await _accountService.SendConfirmUrlAsync(model.Email, url);
                }
                userModel.Errors.Add("User with such email is alrady exists or password is too short");
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                userModel.Errors.Add(error.ErrorMessage);
            }

            return Ok(userModel);
        }

        [HttpPost]
        public async Task ConfirmEmail(string email)
        {
            await _accountService.ConfirmEmail(email);
        }

        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody] LoginModel model)
        {
            var userModel = new UserModel
            {
                Email = model.Email,
                Password = model.Password
            };
            var succeed = await _accountService.LoginAsync(userModel);

            if (!succeed)
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
        public async Task SignOut()
        {
            await _accountService.LogoutAsync();
        }
    }
}
