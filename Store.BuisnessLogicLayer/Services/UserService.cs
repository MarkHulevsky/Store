using Microsoft.AspNetCore.Identity;
using Store.BuisnessLogic.Helpers;
using Store.BuisnessLogicLayer.Models.Base;
using Store.BuisnessLogicLayer.Models.Filters;
using Store.BuisnessLogicLayer.Models.Users;
using Store.BuisnessLogicLayer.Services.Interfaces;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Filters;
using Store.DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using static Store.DataAccessLayer.Entities.Enums.Enums;

namespace Store.BuisnessLogicLayer.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly Mapper<User, UserModel> _userModelMapper = new Mapper<User, UserModel>();
        private readonly Mapper<PagingModel, Paging> _pagingMapper = new Mapper<PagingModel, Paging>();
        private readonly Mapper<UserModel, User> _userMapper = new Mapper<UserModel, User>();
        private readonly Mapper<UserRequestFilterModel, UserRequestFilter> _filterMapper =
            new Mapper<UserRequestFilterModel, UserRequestFilter>();

        public UserService(IUserRepository userRepository, UserManager<User> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public async Task<UserModel> GetCurrentAsync(ClaimsPrincipal httpUser)
        {
            var user = await _userManager.GetUserAsync(httpUser);
            return _userModelMapper.Map(new UserModel(), user);
        }

        public List<UserModel> Filter(UserRequestFilterModel filterModel)
        {
            var statuses = new List<UserStatus>();
            foreach (var statusModel in filterModel.Statuses)
            {
                statuses.Add((UserStatus)statusModel);
            }
            var paging = _pagingMapper.Map(new Paging(), filterModel.Paging);
            var filter = _filterMapper.Map(new UserRequestFilter(), filterModel);
            filter.Statuses = statuses;

            var users = _userRepository.Filter(filter);
            var userModels = new List<UserModel>();

            foreach (var user in users)
            {
                var userModel = _userModelMapper.Map(new UserModel(), user);
                userModels.Add(userModel);
            }
            return userModels;
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
            if (user.Status == UserStatus.Active)
            {
                user.Status = UserStatus.Blocked;
                await _userRepository.UpdateAsync(user);
                return;
            }
            user.Status = UserStatus.Active;
            await _userRepository.UpdateAsync(user);
            return;
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

        public async Task RemoveAsync(UserModel userModel)
        {
            var user = await _userRepository.FindByEmailAsync(userModel.Email);
            await _userRepository.RemoveAsync(user.Id);
        }
    }
}
