using Store.BuisnessLogicLayer.Models.Users;
using System.Collections.Generic;

namespace Store.BuisnessLogic.Models.Filters.ResponseFilters
{
    public class UserResponseFilterModel : BaseResponseFilter
    {
        public List<UserModel> Users { get; set; } = new List<UserModel>();
    }
}
