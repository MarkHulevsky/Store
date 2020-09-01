using Store.BuisnessLogic.Models.Base;
using Store.BuisnessLogic.Models.Users;
using System;
using System.Collections.Generic;
using static Shared.Enums.Enums;

namespace Store.BuisnessLogic.Models.Orders
{
    public class OrderModel : BaseModel
    {
        public DateTime CreationDate { get; set; }
        public OrderStatus Status { get; set; }
        public UserModel User { get; set; }
        public List<OrderItemModel> OrderItems { get; set; }
        public OrderModel()
        {
            OrderItems = new List<OrderItemModel>();
        }
    }
}
