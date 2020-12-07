using GraphQL.Types;
using Store.BuisnessLogic.Models.Orders;
using Store.Presentation.GraphQL.Models.PrintingEdition;

namespace Store.Presentation.GraphQL.Models.Orders
{
    public class OrderItemType : ObjectGraphType<OrderItemModel>
    {
        public OrderItemType()
        {
            Field(x => x.Id);
            Field<ListGraphType<StringGraphType>>(nameof(OrderItemModel.Errors));
            Field(x => x.CreationDate);
            Field(x => x.Amount);
            Field(x => x.Count);
            Field(x => x.OrderId);
            Field(x => x.PrintingEditionId);
            Field<PrintingEditionType>(nameof(OrderItemModel.PrintingEdition));
        }
    }
}
