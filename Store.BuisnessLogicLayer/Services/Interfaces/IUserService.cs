﻿using Store.BuisnessLogic.Models.Filters.ResponseFilters;
using Store.BuisnessLogicLayer.Models.Base;
using Store.BuisnessLogicLayer.Models.Filters;
using Store.BuisnessLogicLayer.Models.Users;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Store.BuisnessLogicLayer.Services.Interfaces
{
    public interface IUserService
    {
        UserResponseFilterModel Filter(UserRequestFilterModel filterModel);
        Task<UserModel> GetCurrentAsync(ClaimsPrincipal user);
        Task<BaseModel> EditAsync(UserModel userModel);
        Task ChangeStatusAsync(UserModel userModel);
        Task RemoveAsync(Guid userId);
        Task<List<UserModel>> GetAllAsync();
    }
}
