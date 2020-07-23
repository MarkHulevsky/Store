using Store.DataAccess.Filters.ResponseFilters;
using Store.DataAccessLayer.Entities;
using System.Collections.Generic;

namespace Store.DataAccess.Filters
{
    public class UserResponseFilter: BaseResponseFilter
    {
        public IEnumerable<User> Users { get; set; } = new List<User>();
    }
}
