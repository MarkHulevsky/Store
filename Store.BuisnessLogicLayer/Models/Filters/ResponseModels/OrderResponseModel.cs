using Store.BuisnessLogic.Models.Orders;
using System.Collections.Generic;

namespace Store.BuisnessLogic.Models.Filters.ResponseFilters
{
    public class OrderResponseModel : BaseResponseModel
    {
        public List<OrderModel> Orders { get; set; }
        public OrderResponseModel()
        {
            Orders = new List<OrderModel>();
        }
    }
}
