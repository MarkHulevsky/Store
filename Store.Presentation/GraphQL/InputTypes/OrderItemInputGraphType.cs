using GraphQL.Types;
using Store.BuisnessLogic.Models.Orders;

namespace Store.Presentation.GraphQL.InputTypes
{
    public class OrderItemInputGraphType: InputObjectGraphType<OrderItemModel>
    {
        public OrderItemInputGraphType()
        {
            Field(x => x.Id);
            Field<ListGraphType<StringGraphType>>(nameof(OrderItemModel.Errors));
            Field(x => x.CreationDate);
            Field(x => x.Amount);
            Field(x => x.Count);
            Field(x => x.OrderId);
            Field(x => x.PrintingEditionId);
            Field<PrintingEditionInputGraphType>(nameof(OrderItemModel.PrintingEdition));
        }
    }
}
