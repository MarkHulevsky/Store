using Store.BuisnessLogic.Models.Filters;
using Store.BuisnessLogic.Models.Filters.ResponseFilters;
using Store.BuisnessLogic.Models.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.BuisnessLogic.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseModel> FilterAsync(UserRequestModel userRequestModel);
        Task<UserModel> GetCurrentAsync();
        Task<List<string>> EditAsync(EditProfileModel editProfileModel);
        Task ChangeStatusAsync(UserModel userModel);
        Task<List<string>> RemoveAsync(string userId);
    }
}
