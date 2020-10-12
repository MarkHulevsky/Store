using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.BuisnessLogic.Helpers;
using Store.BuisnessLogic.Helpers.Interfaces;
using Store.BuisnessLogic.Models.Account;
using Store.BuisnessLogic.Models.Base;
using Store.BuisnessLogic.Models.Users;
using Store.BuisnessLogic.Services.Interfaces;
using Store.DataAccess.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.BuisnessLogic.Services
{
    public class AccountService : IAccountService
    {
        private const string RESET_PASSWORD_SUBJECT = "Reset password";
        private const string RESET_PASSWORD_BODY = "Your new password:";
        private const string CONFIRM_EMAIL_SUBJECT = "Confirm regestration";
        private const string CONFIRM_EMAIL_BODY = "To complete registration follow the ";
        private const string USER_ROLE_NAME = "user";
        private const string EMAIL_IS_NOT_CONFIRMED_ERROR = "Email is not confirmed";
        private const string USER_NOT_FOUND_ERROR = "No user with such email";
        private const string INCORRECT_LOGIN_DATA_ERROR = "Icorrect password or email";
        private const string USER_IS_BLOCKED_ERROR = "Your account was blocked by administrator";
        private const string USER_IS_REMOVED_ERROR = "Your account was removed";

        private readonly IEmailProvider _emailProvider;
        private readonly IUrlHelper _urlHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly Mapper<User, UserModel> _userModelMapper;
        private readonly Mapper<RegisterModel, User> _userMapper;
        private readonly IJwtProvider _jwtProvider;

        public AccountService(IEmailProvider emailProvider,
            UserManager<User> userManager, SignInManager<User> signInManager, IUrlHelper urlHelper,
            IHttpContextAccessor httpContextAccessor, IJwtProvider jwtProvider)
        {
            _emailProvider = emailProvider;
            _userManager = userManager;
            _signInManager = signInManager;
            _urlHelper = urlHelper;
            _httpContextAccessor = httpContextAccessor;
            _userModelMapper = new Mapper<User, UserModel>();
            _userMapper = new Mapper<RegisterModel, User>();
            _jwtProvider = jwtProvider;
        }

        public async Task<List<string>> ResetPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var errors = new List<string>();
            if (user == null)
            {
                errors.Add(USER_NOT_FOUND_ERROR);
                return errors;
            }
            var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            if (!isEmailConfirmed)
            {
                errors.Add(EMAIL_IS_NOT_CONFIRMED_ERROR);
                return errors;
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var newPassword = PasswordGenerator.GeneratePassword();
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    errors.Add(error.Description);
                }
                return errors;
            }
            var subject = RESET_PASSWORD_SUBJECT;
            var body = $"{RESET_PASSWORD_BODY} {newPassword}";
            await _emailProvider.SendAsync(email, subject, body);
            return errors;
        }

        public async Task<List<string>> RegisterAsync(RegisterModel registerModel)
        {
            var user = _userMapper.Map(registerModel);
            user.UserName = registerModel.Email;
            var result = await _userManager.CreateAsync(user, user.Password);
            var errors = new List<string>();
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    errors.Add(error.Description);
                }
                return errors;
            }
            await _userManager.AddToRoleAsync(user, USER_ROLE_NAME);
            await SendConfirmUrlAsync(registerModel.Email);
            return errors;
        }

        public async Task<List<string>> ConfirmEmail(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var errors = new List<string>();
            if (user == null)
            {
                errors.Add(USER_NOT_FOUND_ERROR);
                return errors;
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    errors.Add(error.Description);
                }
                return errors;
            }
            return errors;
        }

        public async Task<BaseModel> LoginAsync(LoginModel loginModel)
        {
            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            var userModel = new UserModel();
            if (user == null)
            {
                userModel.Errors.Add(USER_NOT_FOUND_ERROR);
                return userModel;
            }
            if (!user.IsActive)
            {
                userModel.Errors.Add(USER_IS_BLOCKED_ERROR);
            }
            if (!user.EmailConfirmed)
            {
                userModel.Errors.Add(EMAIL_IS_NOT_CONFIRMED_ERROR);
            }
            if (user.IsRemoved)
            {
                userModel.Errors.Add(USER_IS_REMOVED_ERROR);
            }
            if (userModel.Errors.Any())
            {
                return userModel;
            }
            var result = await _signInManager.PasswordSignInAsync(user, loginModel.Password, false, false);
            if (!result.Succeeded)
            {
                userModel.Errors.Add(INCORRECT_LOGIN_DATA_ERROR);
                return userModel;
            }
            userModel = _userModelMapper.Map(user);
            userModel.Roles = await GetRolesAsync(userModel.Email);
            _jwtProvider.SetToken(userModel);
            return userModel;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        private async Task<List<string>> GetRolesAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return null;
            }
            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }

        private async Task SendConfirmUrlAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return;
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var url = _urlHelper.Action("ConfirmEmail", "Account",
                new { email, token }, _httpContextAccessor.HttpContext.Request.Scheme);
            var subject = CONFIRM_EMAIL_SUBJECT;
            var body = $"{CONFIRM_EMAIL_BODY} <a href='{url}'>link</a>.";
            await _emailProvider.SendAsync(email, subject, body);
        }
    }
}
