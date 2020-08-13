using Store.BuisnessLogicLayer.Models.Base;
using Store.BuisnessLogicLayer.Models.Orders;
using System;

namespace Store.BuisnessLogic.Models.Orders
{
    public class CartModel : BaseModel
    {
        public Guid UserId { get; set; }
        public OrderModel Order { get; set; }
    }
}
