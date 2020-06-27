using Store.BuisnessLogicLayer.Models.Base;
using System.Collections.Generic;

namespace Store.BuisnessLogicLayer.Models.Orders
{
    public class OrderModel: BaseModel
    {
        public IEnumerable<OrderItemModel> OrderItems { get; set; } = new List<OrderItemModel>();
    }
}
