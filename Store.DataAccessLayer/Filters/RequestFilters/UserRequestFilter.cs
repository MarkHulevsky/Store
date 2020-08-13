using Store.DataAccess.Filters.RequestFilters;
using System.Collections.Generic;

namespace Store.DataAccessLayer.Filters
{
    public class UserRequestFilter : BaseRequestFilter
    {
        public string PropName { get; set; }
        public List<bool> Statuses { get; set; } = new List<bool>();
        public string SearchString { get; set; }
    }
}
