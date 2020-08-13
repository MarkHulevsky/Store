using Store.BuisnessLogicLayer.Models.Base;
using Store.BuisnessLogicLayer.Models.Users;
using System;
using System.Collections.Generic;
using static Store.BuisnessLogicLayer.Models.Enums.Enums;

namespace Store.BuisnessLogicLayer.Models.Orders
{
    public class OrderModel : BaseModel
    {
        public DateTime CreationDate { get; set; }
        public OrderStatus Status { get; set; }
        public UserModel User { get; set; }
        public IEnumerable<OrderItemModel> OrderItems { get; set; } = new List<OrderItemModel>();
    }
}
