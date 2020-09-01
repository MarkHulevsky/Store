using Store.DataAccess.Entities;
using Store.DataAccess.Filters.ResponseFilters;
using System.Collections.Generic;

namespace Store.DataAccess.Filters
{
    public class UserResponseDataModel : BaseResponseDataModel
    {
        public IEnumerable<User> Users { get; set; }
        public UserResponseDataModel()
        {
            Users = new List<User>();
        }
    }
}
