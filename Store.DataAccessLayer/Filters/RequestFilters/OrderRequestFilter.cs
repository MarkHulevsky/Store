using Store.DataAccess.Filters.RequestFilters;
using System.Collections.Generic;
using static Store.DataAccessLayer.Entities.Enums.Enums;

namespace Store.DataAccessLayer.Filters
{
    public class OrderRequestFilter: BaseRequestFilter
    {
        public string PropName { get; set; }
        public List<OrderStatus> OrderStatuses { get; set; } = new List<OrderStatus>();

        public OrderRequestFilter()
        {
            Paging = new Paging
            {
                ItemsCount = 15,
                CurrentPage = 0
            };
        }
    }
}
