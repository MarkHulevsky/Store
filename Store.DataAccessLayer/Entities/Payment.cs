using Store.DataAccessLayer.Entities.Base;
using System;

namespace Store.DataAccessLayer.Entities
{
    public class Payment: BaseEntity
    {
        public string TransactionId { get; set; }
    }
}
