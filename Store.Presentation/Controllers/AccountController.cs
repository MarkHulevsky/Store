using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Store.BuisnessLogic.Helpers.Interfaces;
using Store.BuisnessLogic.Models.Account;
using Store.BuisnessLogic.Models.Token;
using Store.BuisnessLogic.Services.Interfaces;
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
            var jwtToken = _jwtProvider.RefreshToken(refreshTokenModel);
            return Ok(jwtToken);
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] RegisterModel registerModel)
        {
            var result = await _accountService.RegisterAsync(registerModel);
            return Ok(result);
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
            var userModel = await _accountService.LoginAsync(loginModel);
            return Ok(userModel);
        }

        [HttpPost]
        public async Task SignOut()
        {
            await _accountService.LogoutAsync();
        }
    }
}
