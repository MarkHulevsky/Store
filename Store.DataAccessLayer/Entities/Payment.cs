using Store.DataAccessLayer.Entities.Base;

namespace Store.DataAccessLayer.Entities
{
    public class Payment : BaseEntity
    {
        public string TransactionId { get; set; }
    }
}
