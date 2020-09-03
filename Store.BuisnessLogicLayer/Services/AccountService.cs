using Microsoft.AspNetCore.Identity;
using Store.BuisnessLogic.Helpers;
using Store.BuisnessLogic.Helpers.Interfaces;
using Store.BuisnessLogic.Models.Base;
using Store.BuisnessLogic.Models.Users;
using Store.BuisnessLogic.Services.Interfaces;
using Store.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.BuisnessLogic.Services
{
    public class AccountService : IAccountService
    {
        private const string RESET_PASSWORD_SUBJECT = "Reset password";
        private const string RESET_PASSWORD_BODY = "Your new password:";
        private const string CONFIRM_EMAIL_SUBJECT = "Confirm regestration";
        private const string CONFIRM_EMAIL_BODY = "To complete registration follow the link:";
        private const string USER_ROLE_NAME = "user";
        private const string USER_NOT_FOUND_ERROR = "No user with such email";
        private const string INCORRECT_LOGIN_DATA_ERROR = "Icorrect password or email";

        private readonly IEmailProvider _emailProvider;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly Mapper<User, UserModel> _userModelMapper;
        private readonly Mapper<UserModel, User> _userMapper;

        public AccountService(IEmailProvider emailProvider,
            UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _emailProvider = emailProvider;
            _userManager = userManager;
            _signInManager = signInManager;
            _userModelMapper = new Mapper<User, UserModel>();
            _userMapper = new Mapper<UserModel, User>();
        }

        public async Task<BaseModel> ResetPasswordAsync(string email, string token, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return null;
            }
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (!result.Succeeded)
            {
                var errors = new List<string>();
                foreach (var error in result.Errors)
                {
                    errors.Add(error.Description);
                }
                return new BaseModel
                {
                    Errors = errors
                };
            }
            var subject = RESET_PASSWORD_SUBJECT;
            var body = $"{RESET_PASSWORD_BODY} {newPassword}";
            await _emailProvider.SendAsync(email, subject, body);
            return new BaseModel();
        }

        public async Task<string> GetForgotPasswordTokenAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return null;
            }
            var user = await _userManager.FindByEmailAsync(email);
            var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            if (user == null || !isEmailConfirmed)
            {
                return null;
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return token;
        }

        public async Task<List<string>> GetRolesAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return null;
            }
            var roles = await _userManager.GetRolesAsync(user);
            return roles as List<string>;
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

        public async Task<IdentityResult> RegisterAsync(UserModel userModel)
        {
            var user = _userMapper.Map(userModel);
            user.UserName = userModel.Email;
            var result = await _userManager.CreateAsync(user, user.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, USER_ROLE_NAME);
            }
            return result;
        }

        public async Task SendConfirmUrlAsync(string email, string url)
        {
            var subject = CONFIRM_EMAIL_SUBJECT;
            var body = $"{CONFIRM_EMAIL_BODY} {url}";
            await _emailProvider.SendAsync(email, subject, body);
        }

        public async Task<BaseModel> ConfirmEmail(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return null;
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                var userModel = _userModelMapper.Map(user);
                await LoginAsync(userModel);
                return new BaseModel();
            }
            var errors = new List<string>();
            foreach (var error in result.Errors)
            {
                errors.Add(error.Description);
            }
            return new BaseModel
            {
                Errors = errors
            };
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return token;
        }

        public async Task<BaseModel> LoginAsync(UserModel userModel)
        {
            var user = await _userManager.FindByEmailAsync(userModel.Email);
            if (user == null)
            {
                var errors = new List<string>
                {
                    USER_NOT_FOUND_ERROR
                };
                return new BaseModel
                {
                    Errors = errors
                };

            }
            user.Password = userModel.Password;
            var result = await _signInManager.PasswordSignInAsync(user, user.Password, false, false);
            if (!result.Succeeded)
            {
                var errors = new List<string>
                {
                    INCORRECT_LOGIN_DATA_ERROR
                };
                return new BaseModel
                {
                    Errors = errors
                };
            }
            return userModel;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
