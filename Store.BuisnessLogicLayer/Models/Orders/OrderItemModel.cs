using Store.BuisnessLogicLayer.Models.Base;
using Store.BuisnessLogicLayer.Models.PrintingEditions;
using System;

namespace Store.BuisnessLogicLayer.Models.Orders
{
    public class OrderItemModel: BaseModel
    {
        public Guid PrintingEditionId { get; set; }
        public Guid OrderId { get; set; }
        public int Amount { get; set; }
        public int Count { get; set; }
        public PrintingEditionModel PrintingEdition { get; set; }
    }
}
