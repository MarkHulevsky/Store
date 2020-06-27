using System.Collections.Generic;
using static Store.DataAccessLayer.Entities.Enums.Enums;

namespace Store.DataAccessLayer.Filters
{
    public class OrderRequestFilter
    {
        public string PropName { get; set; }
        public SortType SortType { get; set; }
        public List<OrderStatus> OrderStatuses { get; set; }
        public Paging Paging { get; set; }

        public OrderRequestFilter()
        {
            Paging = new Paging
            {
                ItemsCount = 15,
                Number = 0
            };
        }
    }
}
