using GraphQL.Types;
using Store.BuisnessLogic.Models.Orders;

namespace Store.Presentation.GraphQL.InputTypes
{
    public class OrderInputGraphType: InputObjectGraphType<OrderModel>
    {
        public OrderInputGraphType()
        {
            Field(x => x.Id, nullable: true);
            Field(x => x.CreationDate, nullable: true);
            Field<ListGraphType<StringGraphType>>(nameof(OrderModel.Errors));
            Field<IntGraphType>(nameof(OrderModel.Status), resolve: context => (int)context.Source.Status);
            Field<ListGraphType<OrderItemInputGraphType>>(nameof(OrderModel.OrderItems));
        }
    }
}
