using Store.DataAccess.Filters.RequestFilters;
using System.Collections.Generic;

namespace Store.DataAccess.Filters
{
    public class UserRequestDataModel : BaseRequetDataModel
    {
        public string SortPropertyName { get; set; }
        public List<bool> Statuses { get; set; }
        public string SearchString { get; set; }
        public UserRequestDataModel()
        {
            Statuses = new List<bool>();
        }
    }
}
