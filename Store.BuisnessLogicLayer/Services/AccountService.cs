using Microsoft.AspNetCore.Identity;
using Store.BuisnessLogic.Helpers;
using Store.BuisnessLogic.Helpers.Interfaces;
using Store.BuisnessLogicLayer.Models.Base;
using Store.BuisnessLogicLayer.Models.Users;
using Store.BuisnessLogicLayer.Services.Interfaces;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.BuisnessLogicLayer.Services
{
    public class AccountService: IAccountService
    {
        private const string userRoleName = "user";
        private readonly IUserRepository _userRepository;
        private readonly IEmailHalper _emailHalper;
        private readonly Mapper<User, UserModel> _userModelMapper = new Mapper<User, UserModel>();
        private readonly Mapper<UserModel, User> _userMapper = new Mapper<UserModel, User>();

        public AccountService(IUserRepository userRepository, IEmailHalper emailHalper)
        {
            _userRepository = userRepository;
            _emailHalper = emailHalper;
        }

        public async Task<BaseModel> ResetPasswordAsync(string email, string token, string newPassword)
        {
            var result = await _userRepository.ResetPasswordAsync(email, token, newPassword);
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
            var subject = "Reset password";
            var body = $"Your new password: {newPassword}";
            await _emailHalper.SendAsync(email, subject, body);
            return new BaseModel();
        }

        public async Task<string> GetForgotPasswordTokenAsync(string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                return await _userRepository.GetFrogotPasswordTokenAsync(email);
            }
            return string.Empty;
        }

        public async Task<string> GetRoleNameAsync(string email)
        {
            var user = await _userRepository.FindByEmailAsync(email);
            if (user != null)
            {
                return await _userRepository.GetRoleNameAsync(user);
            }
            return string.Empty;
        }

        public async Task<UserModel> FindByEmailAsync(string email)
        {
            var user = await _userRepository.FindByEmailAsync(email);
            if (user != null)
            {
                return _userModelMapper.Map(new UserModel(), user);
            }
            return null;
        }

        public async Task<IdentityResult> RegisterAsync(UserModel userModel)
        {
            var user = _userMapper.Map(new User(), userModel);
            user.UserName = user.Email;
            var result = await _userRepository.CreateAsync(user);
            if (result.Succeeded)
            {
                await _userRepository.AddToRoleAsync(user, userRoleName);
            }
            return result;
        }

        public async Task SendConfirmUrlAsync(string email, string url)
        {
            var subject = "Confirm regestration";
            var body = $"To complete registration follow the link: {url}";
            await _emailHalper.SendAsync(email, subject, body);
        }

        public async Task ConfirmEmail(string userEmail)
        {
            var user = await _userRepository.FindByEmailAsync(userEmail);
            if (user != null)
            {
                user.EmailConfirmed = true;
                await _userRepository.UpdateAsync(user);
                var userModel = _userModelMapper.Map(new UserModel(), user);
                await LoginAsync(userModel);
            }
        }

        public async Task<bool> LoginAsync(UserModel userModel)
        {
            var user = await  _userRepository.FindByEmailAsync(userModel.Email);
            if (user != null)
            {
                user.Password = userModel.Password;
                var result = await _userRepository.SignInAsync(user);
                return result.Succeeded;
            }
            return false;
        }

        public async Task LogoutAsync()
        {
            await _userRepository.SignOutAsync();
        }
    }
}
