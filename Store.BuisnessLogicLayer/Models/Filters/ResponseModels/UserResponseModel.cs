using Store.BuisnessLogic.Models.Users;
using System.Collections.Generic;

namespace Store.BuisnessLogic.Models.Filters.ResponseFilters
{
    public class UserResponseModel : BaseResponseModel
    {
        public List<UserModel> Users { get; set; }
        public UserResponseModel()
        {
            Users = new List<UserModel>();
        }
    }
}
