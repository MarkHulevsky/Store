using Microsoft.AspNetCore.Identity;
using Store.BuisnessLogic.Helpers;
using Store.BuisnessLogic.Helpers.Mappers.RequestFilterMappers;
using Store.BuisnessLogic.Helpers.Mappers.ResponseFilterMappers;
using Store.BuisnessLogic.Models.Filters.ResponseFilters;
using Store.BuisnessLogicLayer.Models.Base;
using Store.BuisnessLogicLayer.Models.Filters;
using Store.BuisnessLogicLayer.Models.Users;
using Store.BuisnessLogicLayer.Services.Interfaces;
using Store.DataAccess.Filters;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Filters;
using Store.DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Store.BuisnessLogicLayer.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly Mapper<User, UserModel> _userModelMapper = new Mapper<User, UserModel>();
        private readonly Mapper<UserModel, User> _userMapper = new Mapper<UserModel, User>();

        public UserService(IUserRepository userRepository, UserManager<User> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public async Task<UserModel> GetCurrentAsync(ClaimsPrincipal httpUser)
        {
            var user = await _userManager.GetUserAsync(httpUser);
            var userModel =  _userModelMapper.Map(new UserModel(), user);
            userModel.Roles = await _userManager.GetRolesAsync(user) as List<string>;
            return userModel;
        }

        public UserResponseFilterModel Filter(UserRequestFilterModel filterModel)
        {
            var filter = UserRequestFilterMapper.Map(filterModel);
            var userResponse = _userRepository.Filter(filter);
            var userResponseModel = UserResponseFilterMapper.Map(userResponse);
            return userResponseModel;
        }

        public async Task<List<UserModel>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var userModels = new List<UserModel>();

            users.ForEach(u =>
            {
                var userModel = _userModelMapper.Map(new UserModel(), u);
                userModels.Add(userModel);
            });

            return userModels;
        }

        public async Task ChangeStatusAsync(UserModel userModel)
        {
            var user = await _userRepository.FindByEmailAsync(userModel.Email);
            user.IsActive = !user.IsActive;
            await _userRepository.UpdateAsync(user);
        }

        public async Task<BaseModel> EditAsync(UserModel userModel)
        {
            var user = await _userRepository.GetAsync(userModel.Id);
            if (user != null)
            {
                user = _userMapper.Map(new User(), userModel);
                await _userRepository.UpdateAsync(user);
                return new BaseModel();
            }
            var baseModel = new BaseModel();
            baseModel.Errors.Add("No such user");
            return baseModel;
        }

        public async Task RemoveAsync(Guid userId)
        {
            await _userRepository.RemoveAsync(userId);
        }
    }
}
