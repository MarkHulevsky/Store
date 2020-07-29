using Store.BuisnessLogicLayer.Models.Base;
using System.Collections.Generic;
using static Store.BuisnessLogicLayer.Models.Enums.Enums;

namespace Store.BuisnessLogicLayer.Models.Orders
{
    public class OrderModel: BaseModel
    {
        public OrderStatus Status { get; set; }
        public IEnumerable<OrderItemModel> OrderItems { get; set; } = new List<OrderItemModel>();
    }
}
