using Microsoft.AspNetCore.Identity;
using Store.BuisnessLogic.Helpers;
using Store.BuisnessLogic.Helpers.Mappers.RequestFilterMappers;
using Store.BuisnessLogic.Helpers.Mappers.ResponseFilterMappers;
using Store.BuisnessLogic.Models.Base;
using Store.BuisnessLogic.Models.Filters;
using Store.BuisnessLogic.Models.Filters.ResponseFilters;
using Store.BuisnessLogic.Models.Users;
using Store.BuisnessLogic.Services.Interfaces;
using Store.DataAccess.Entities;
using Store.DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Store.BuisnessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly Mapper<User, UserModel> _userModelMapper;

        public UserService(IUserRepository userRepository, UserManager<User> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _userModelMapper = new Mapper<User, UserModel>();
        }

        public async Task<UserModel> GetCurrentAsync(ClaimsPrincipal httpUser)
        {
            var user = await _userManager.GetUserAsync(httpUser);
            var userModel = _userModelMapper.Map(user);
            userModel.Roles = await _userManager.GetRolesAsync(user) as List<string>;
            return userModel;
        }

        public async Task<UserResponseModel> FilterAsync(UserRequestModel filterModel)
        {
            var filter = UserRequestMapper.Map(filterModel);
            var userResponse = await _userRepository.FilterAsync(filter);
            var userResponseModel = UserResponseFilterMapper.Map(userResponse);
            return userResponseModel;
        }

        public async Task ChangeStatusAsync(UserModel userModel)
        {
            var user = await _userRepository.FindByEmailAsync(userModel.Email);
            user.IsActive = !user.IsActive;
            await _userManager.UpdateAsync(user);
        }

        public async Task<BaseModel> EditAsync(UserModel userModel)
        {
            var user = await _userRepository.GetAsync(userModel.Id);
            if (user == null)
            {
                var baseModel = new BaseModel();
                baseModel.Errors.Add("No such user");
                return baseModel;
            }
            user.FirstName = userModel.FirstName;
            user.LastName = userModel.LastName;
            user.Email = userModel.Email;
            var updateResult = await _userManager.UpdateAsync(user);
            var result = new BaseModel();
            if (updateResult.Succeeded)
            {
                return result;
            }
            foreach(var error in updateResult.Errors)
            {
                result.Errors.Add(error.Description);
            }
            return result;
        }

        public async Task RemoveAsync(Guid userId)
        {
            await _userRepository.RemoveAsync(userId);
        }
    }
}
