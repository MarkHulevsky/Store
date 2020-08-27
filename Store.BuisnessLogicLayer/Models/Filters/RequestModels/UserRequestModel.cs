using System.Collections.Generic;

namespace Store.BuisnessLogic.Models.Filters
{
    public class UserRequestModel : BaseRequestModel
    {
        public string SortPropertyName { get; set; }
        public string SearchString { get; set; }
        public IEnumerable<bool> Statuses { get; set; }
        public UserRequestModel()
        {
            Statuses = new List<bool>();
        }
    }
}
