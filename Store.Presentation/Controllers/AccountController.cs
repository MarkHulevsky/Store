using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Store.BuisnessLogic.Helpers.Interfaces;
using Store.BuisnessLogic.Models.Account;
using Store.BuisnessLogic.Models.Token;
using Store.BuisnessLogic.Models.Users;
using Store.BuisnessLogic.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Presentation.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IJwtProvider _jwtProvider;
        private readonly IConfiguration _configuration;

        public AccountController(IAccountService accountService, IJwtProvider jwtProvider,
            IConfiguration configuration)
        {
            _accountService = accountService;
            _jwtProvider = jwtProvider;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel forgotPasswordModel)
        {
            var baseModel = await _accountService.ResetPasswordAsync(forgotPasswordModel.Email);
            return Ok(baseModel);
        }

        [HttpPost]
        public IActionResult RefreshToken([FromBody] JwtTokenModel refreshTokenModel)
        {
            var principal = _jwtProvider.GetPrincipalFromExpiredToken(refreshTokenModel.AccessToken);
            var jwtToken = new JwtTokenModel
            {
                AccessToken = _jwtProvider.GenerateJwtTokenWithClaims(principal.Claims),
                RefreshToken = _jwtProvider.GenerateRefreshToken()
            };
            _jwtProvider.SetCookieTokenResponse(jwtToken);
            return Ok(jwtToken);
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] RegisterModel registerModel)
        {
            var userModel = new UserModel();
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    userModel.Errors.Add(error.ErrorMessage);
                }
                return Ok(userModel);
            }
            var result = await _accountService.RegisterAsync(registerModel);
            if (result.Errors.Count != 0)
            {
                return Ok(result);
            }
            await _accountService.SendConfirmUrlAsync(registerModel.Email);
            return Ok(userModel);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            await _accountService.ConfirmEmail(email, token);
            var redirectUrl = $"{_configuration.GetSection("ClientSideOptions")["Url"]}/account/email-confirmed";
            return Redirect(redirectUrl);
        }

        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody] LoginModel loginModel)
        {
            var result = await _accountService.LoginAsync(loginModel);
            if (result.Errors.Count != 0)
            {
                return Ok(result);
            }
            var userModel = await _accountService.FindByEmailAsync(loginModel.Email);
            userModel.Roles = await _accountService.GetRolesAsync(loginModel.Email);
            var jwtToken = new JwtTokenModel
            {
                AccessToken = await _jwtProvider.GetTokenAsync(userModel),
                RefreshToken = _jwtProvider.GenerateRefreshToken()
            };
            _jwtProvider.SetCookieTokenResponse(jwtToken);
            return Ok(userModel);
        }

        [HttpPost]
        public async Task SignOut()
        {
            await _accountService.LogoutAsync();
        }
    }
}
