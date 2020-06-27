using Microsoft.AspNetCore.Mvc;
using Store.BuisnessLogic.Helpers;
using Store.BuisnessLogicLayer.Helpers;
using Store.BuisnessLogicLayer.Models.Users;
using Store.BuisnessLogicLayer.Services.Interfaces;
using Store.Presentation.Helpers.Interfaces;
using Store.Presentation.Models.AccountModels;
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

        
        [HttpPost]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel model)
        {
            var userModel = new UserModel();
            if (ModelState.IsValid)
            {
                var token = await _accountService.GetForgotPasswordTokenAsync(model.Email);
                string newPassword = PasswordGenerator.GeneratePassword();

                var result = await _accountService.ResetPasswordAsync(model.Email, token, newPassword);                
                if (result.Errors.Count != 0)
                {
                    return BadRequest(result.Errors);
                }

                return Ok();
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                userModel.Errors.Add(error.ErrorMessage);
            }

            return Ok(userModel);
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] RegisterViewModel model)
        {
            var userModel = new UserModel();
            if (ModelState.IsValid)
            { 
                var userModelMapper = new Mapper<RegisterViewModel, UserModel>();
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
        public async Task<IActionResult> SignIn([FromBody] LoginViewModel model)
        {
            var userModel = new UserModel();
            if (ModelState.IsValid)
            {
                var userModelMapper = new Mapper<LoginViewModel, UserModel>();
                userModel = userModelMapper.Map(new UserModel(), model);

                var succeed = await _accountService.LoginAsync(userModel);
                if (succeed)
                {
                    var token = new
                    {
                        access_token = await _jwtHelper.GetTokenAsync(userModel)
                    };
                    return Json(token);
                }
                userModel.Errors.Add("Invalid password or email");
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                userModel.Errors.Add(error.ErrorMessage);
            }

            return Ok(userModel);
        }

        [HttpPost]
        public async Task<IActionResult> SignOut()
        {
            await _accountService.LogoutAsync();
            return Ok(User == null);
        }
    }
}
