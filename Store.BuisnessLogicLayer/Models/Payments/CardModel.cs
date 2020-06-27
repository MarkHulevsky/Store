using Store.BuisnessLogicLayer.Models.Base;

namespace Store.BuisnessLogic.Models.Payments
{
    public class CardModel: BaseModel
    {
        public string CardNumber { get; set; }
        public int ExpYear { get; set; }
        public int ExpMonth { get; set; }
        public string CVC { get; set; }
    }
}
