using Store.DataAccess.Entities;
using Store.DataAccess.Filters.ResponseFilters;
using System.Collections.Generic;

namespace Store.DataAccess.Filters.ResponseFulters
{
    public class OrderResponseDataModel : BaseResponseDataModel
    {
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
