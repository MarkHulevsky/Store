using Store.BuisnessLogic.Models.Filters;
using System.Collections.Generic;

namespace Store.BuisnessLogicLayer.Models.Filters
{
    public class UserRequestFilterModel : BaseFilterModel
    {
        public string PropName { get; set; }
        public string SearchString { get; set; }
        public IEnumerable<bool> Statuses { get; set; }
    }
}
