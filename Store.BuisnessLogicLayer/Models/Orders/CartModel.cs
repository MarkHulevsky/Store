using Store.BuisnessLogic.Models.Base;
using System;

namespace Store.BuisnessLogic.Models.Orders
{
    public class CartModel : BaseModel
    {
        public Guid UserId { get; set; }
        public OrderModel Order { get; set; }
    }
}
