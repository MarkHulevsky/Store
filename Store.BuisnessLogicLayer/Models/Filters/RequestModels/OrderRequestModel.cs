using System.Collections.Generic;
using static Shared.Enums.Enums;

namespace Store.BuisnessLogic.Models.Filters
{
    public class OrderRequestModel : BaseRequestModel
    {
        public string SortPropertyName { get; set; }
        public List<OrderStatus> OrderStatuses { get; set; }
    }
}
