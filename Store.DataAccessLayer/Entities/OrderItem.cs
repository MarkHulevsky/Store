using Store.DataAccess.Entities.Base;
using System;

namespace Store.DataAccess.Entities
{
    public class OrderItem : BaseEntity
    {
        public int Amount { get; set; }
        public Guid PrintingEditionId { get; set; }
        public virtual PrintingEdition PrintingEdition { get; set; }
        public Guid OrderId { get; set; }
        public virtual Order Order { get; set; }
        public int Count { get; set; }
    }
}
