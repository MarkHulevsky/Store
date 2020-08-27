using Store.BuisnessLogic.Models.Base;
using Store.BuisnessLogic.Models.Filters;
using Store.BuisnessLogic.Models.Filters.ResponseFilters;
using Store.BuisnessLogic.Models.Users;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Store.BuisnessLogic.Services.Interfaces
{
    public interface IUserService
    {
        UserResponseModel Filter(UserRequestModel filterModel);
        Task<UserModel> GetCurrentAsync(ClaimsPrincipal user);
        Task<BaseModel> EditAsync(UserModel userModel);
        Task ChangeStatusAsync(UserModel userModel);
        Task RemoveAsync(Guid userId);
    }
}
