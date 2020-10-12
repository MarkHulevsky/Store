using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Store.BuisnessLogic.Helpers;
using Store.BuisnessLogic.Helpers.Mappers.RequestFilterMappers;
using Store.BuisnessLogic.Helpers.Mappers.ResponseFilterMappers;
using Store.BuisnessLogic.Models.Filters;
using Store.BuisnessLogic.Models.Filters.ResponseFilters;
using Store.BuisnessLogic.Models.Users;
using Store.BuisnessLogic.Services.Interfaces;
using Store.DataAccess.Entities;
using Store.DataAccess.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.BuisnessLogic.Services
{
    public class UserService : IUserService
    {
        private const string NO_SUCH_USER_ERROR = "No such user";

        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly Mapper<User, UserModel> _userModelMapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IUserRepository userRepository, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _userModelMapper = new Mapper<User, UserModel>();
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserModel> GetCurrentAsync()
        {
            var name = _httpContextAccessor.HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(name);
            var userModel = _userModelMapper.Map(user);
            var roles = await _userManager.GetRolesAsync(user);
            userModel.Roles = roles.ToList();
            return userModel;
        }

        public async Task<UserResponseModel> FilterAsync(UserRequestModel userRequestModel)
        {
            var userRequestDataModel = UserRequestMapper.Map(userRequestModel);
            var userResponseDataModel = await _userRepository.FilterAsync(userRequestDataModel);
            var userResponseModel = UserResponseMapper.Map(userResponseDataModel);
            return userResponseModel;
        }

        public async Task ChangeStatusAsync(UserModel userModel)
        {
            var user = await _userManager.FindByEmailAsync(userModel.Email);
            if (user == null)
            {
                return;
            }
            user.IsActive = !user.IsActive;
            await _userManager.UpdateAsync(user);
        }

        public async Task<List<string>> EditAsync(EditProfileModel editProfileModel)
        {
            var user = await _userManager.FindByIdAsync(editProfileModel.Id.ToString());
            var errors = new List<string>();
            if (user == null)
            {
                errors.Add(NO_SUCH_USER_ERROR);
                return errors;
            }
            if (editProfileModel.Password != string.Empty && editProfileModel.Password != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                await _userManager.ResetPasswordAsync(user, token, editProfileModel.Password);
            }
            user.FirstName = editProfileModel.FirstName;
            user.LastName = editProfileModel.LastName;
            user.Email = editProfileModel.Email;
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    errors.Add(error.Description);
                }
            }
            return errors;
        }

        public async Task<List<string>> RemoveAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var errors = new List<string>();
            if (user == null)
            {
                errors.Add(NO_SUCH_USER_ERROR);
                return errors;
            }
            user.IsRemoved = true;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    errors.Add(error.Description);
                }
            }
            return errors;
        }
    }
}
