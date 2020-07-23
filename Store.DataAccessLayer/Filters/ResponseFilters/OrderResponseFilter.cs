using Store.DataAccess.Filters.ResponseFilters;
using Store.DataAccessLayer.Entities;
using System.Collections.Generic;

namespace Store.DataAccess.Filters.ResponseFulters
{
    public class OrderResponseFilter: BaseResponseFilter
    {
        public IEnumerable<Order> Orders { get; set; } = new List<Order>();
    }
}
