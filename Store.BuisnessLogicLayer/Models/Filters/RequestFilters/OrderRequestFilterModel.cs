using Store.BuisnessLogic.Models.Filters;
using System.Collections.Generic;
using static Store.BuisnessLogicLayer.Models.Enums.Enums;

namespace Store.BuisnessLogicLayer.Models.Filters
{
    public class OrderRequestFilterModel : BaseFilterModel
    {
        public string PropName { get; set; }
        public List<OrderStatus> OrderStatuses { get; set; }
    }
}
