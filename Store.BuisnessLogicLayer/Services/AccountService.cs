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

        public AccountService(IEmailProvider emailProvider,
            UserManager<User> userManager, SignInManager<User> signInManager, IUrlHelper urlHelper,
            IHttpContextAccessor httpContextAccessor)
        {
            _emailProvider = emailProvider;
            _userManager = userManager;
            _signInManager = signInManager;
            _urlHelper = urlHelper;
            _httpContextAccessor = httpContextAccessor;
            _userModelMapper = new Mapper<User, UserModel>();
            _userMapper = new Mapper<RegisterModel, User>();
        }

        public async Task<BaseModel> ResetPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            if (user == null || !isEmailConfirmed)
            {
                return null;
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var newPassword = PasswordGenerator.GeneratePassword();
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (!result.Succeeded)
            {
                var baseModel = new BaseModel();
                foreach (var error in result.Errors)
                {
                    baseModel.Errors.Add(error.Description);
                }
                return baseModel;
            }
            var subject = RESET_PASSWORD_SUBJECT;
            var body = $"{RESET_PASSWORD_BODY} {newPassword}";
            await _emailProvider.SendAsync(email, subject, body);
            return new BaseModel();
        }

        public async Task<List<string>> GetRolesAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return null;
            }
            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }

        public async Task<UserModel> FindByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return null;
            }
            var userModel = _userModelMapper.Map(user);
            return userModel;
        }

        public async Task<BaseModel> RegisterAsync(RegisterModel registerModel)
        {
            var user = _userMapper.Map(registerModel);
            user.UserName = registerModel.Email;
            var result = await _userManager.CreateAsync(user, user.Password);
            var baseModel = new BaseModel();
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    baseModel.Errors.Add(error.Description);
                }
            }
            await _userManager.AddToRoleAsync(user, USER_ROLE_NAME);
            return baseModel;
        }

        public async Task SendConfirmUrlAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var url = _urlHelper.Action("ConfirmEmail", "Account",
                new { email, token }, _httpContextAccessor.HttpContext.Request.Scheme);
            var subject = CONFIRM_EMAIL_SUBJECT;
            var body = $"{CONFIRM_EMAIL_BODY} <a href='{url}'>link</a>.";
            await _emailProvider.SendAsync(email, subject, body);
        }

        public async Task<BaseModel> ConfirmEmail(string email, string token)
        {
            var baseModel = new BaseModel();
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                baseModel.Errors.Add(USER_NOT_FOUND_ERROR);
                return baseModel;
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    baseModel.Errors.Add(error.Description);
                }
                return baseModel;
            }
            return baseModel;
        }

        public async Task<BaseModel> LoginAsync(LoginModel loginModel)
        {
            var userModel = new UserModel();
            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            if (user == null)
            {
                userModel.Errors.Add(USER_NOT_FOUND_ERROR);
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
            if (userModel.Errors.Count != 0)
            {
                return userModel;
            }
            user.Password = loginModel.Password;
            var result = await _signInManager.PasswordSignInAsync(user, user.Password, false, false);
            if (!result.Succeeded)
            {
                userModel.Errors.Add(INCORRECT_LOGIN_DATA_ERROR);
            }
            return userModel;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
