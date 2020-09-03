using Store.BuisnessLogic.Models.Base;
using Store.BuisnessLogic.Models.Filters;
using Store.BuisnessLogic.Models.Filters.ResponseFilters;
using Store.BuisnessLogic.Models.Users;
using System;
using System.Threading.Tasks;

namespace Store.BuisnessLogic.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseModel> FilterAsync(UserRequestModel userRequestModel);
        Task<UserModel> GetCurrentAsync(string name);
        Task<BaseModel> EditAsync(UserModel userModel);
        Task ChangeStatusAsync(UserModel userModel);
        Task RemoveAsync(Guid userId);
    }
}
