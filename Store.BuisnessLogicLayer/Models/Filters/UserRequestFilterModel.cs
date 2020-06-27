using Store.BuisnessLogic.Models.Filters;
using System.Collections.Generic;
using static Store.BuisnessLogicLayer.Models.Enums.Enums;

namespace Store.BuisnessLogicLayer.Models.Filters
{
    public class UserRequestFilterModel: BaseFilterModel
    {
        public string PropName { get; set; }
        public string SearchString { get; set; }
        public IEnumerable<UserStatus> Statuses { get; set; }
        public UserRequestFilterModel()
        {
            Paging = new PagingModel
            {
                ItemsCount = 15
            };
        }
    }
}
