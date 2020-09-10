using Dapper.Contrib.Extensions;
using Store.DataAccess.Entities.Base;
using System;
using System.Collections.Generic;
using static Shared.Enums.Enums;

namespace Store.DataAccess.Entities
{
    public class Order : BaseEntity
    {
        public string Description { get; set; }
        public Guid UserId { get; set; }
        [Computed]
        public virtual User User { get; set; }
        public Guid PaymentId { get; set; }
        public OrderStatus Status { get; set; }
        [Computed]
        public virtual List<OrderItem> OrderItems { get; set; }
        public Order()
        {
            OrderItems = new List<OrderItem>();
        }
    }
}
