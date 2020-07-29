using Store.DataAccessLayer.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using static Store.DataAccessLayer.Entities.Enums.Enums;

namespace Store.DataAccessLayer.Entities
{
    public class Order: BaseEntity
    {
        public string Description { get; set; }
        public Guid UserId { get; set; }
        public Guid PaymentId { get; set; }
        public OrderStatus Status { get; set; }
        [NotMapped]
        public virtual List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
