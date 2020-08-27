using Store.DataAccess.Entities;
using Store.DataAccess.Filters.ResponseFilters;
using System.Collections.Generic;

namespace Store.DataAccess.Filters.ResponseFulters
{
    public class OrderResponseDataModel : BaseResponseDataModel
    {
        public IEnumerable<Order> Orders { get; set; }
        public OrderResponseDataModel()
        {
            Orders = new List<Order>();
        }
    }
}
