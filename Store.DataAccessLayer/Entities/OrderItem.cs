using Store.DataAccessLayer.Entities.Base;
using System;
using static Store.DataAccessLayer.Entities.Enums.Enums;

namespace Store.DataAccessLayer.Entities
{
    public class OrderItem: BaseEntity
    {
        public int Amount { get; set; }
        public Guid PrintingEditionId { get; set; }
        public Guid OrderId { get; set; }
        public int Count { get; set; }
    }
}
