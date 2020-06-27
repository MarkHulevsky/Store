using Store.BuisnessLogic.Models.Payments;
using Store.BuisnessLogicLayer.Models.Base;
using System;

namespace Store.BuisnessLogicLayer.Models.Payments
{
    public class PaymentModel: BaseModel
    {
        public Guid OrderId { get; set; }
        public string UserEmail { get; set; }
        public long Amount { get; set; }
        public string Currency { get; set; }
        public CardModel Card { get; set; }
    }
}
