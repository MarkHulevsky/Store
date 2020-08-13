using Store.BuisnessLogicLayer.Models.Orders;
using System.Collections.Generic;

namespace Store.BuisnessLogic.Models.Filters.ResponseFilters
{
    public class OrderResponseFilterModel : BaseResponseFilter
    {
        public List<OrderModel> Orders { get; set; } = new List<OrderModel>();
    }
}
