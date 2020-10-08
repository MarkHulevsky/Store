using Store.DataAccess.Filters.RequestFilters;
using System.Collections.Generic;
using static Shared.Enums.Enums;

namespace Store.DataAccess.Filters
{
    public class OrderRequestDataModel : BaseRequetDataModel
    {
        public string SortPropertyName { get; set; }
        public List<OrderStatus> OrderStatuses { get; set; }
        public OrderRequestDataModel()
        {
            OrderStatuses = new List<OrderStatus>();
        }
    }
}
