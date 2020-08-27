using Store.DataAccess.Entities.Base;

namespace Store.DataAccess.Entities
{
    public class Payment : BaseEntity
    {
        public string TransactionId { get; set; }
    }
}
