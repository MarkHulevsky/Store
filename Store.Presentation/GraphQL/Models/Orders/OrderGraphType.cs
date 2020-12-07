using GraphQL.Types;
using Store.BuisnessLogic.Models.Orders;
using Store.Presentation.GraphQL.Enums;
using Store.Presentation.GraphQL.Models.User;

namespace Store.Presentation.GraphQL.Models.Orders
{
    public class OrderGraphType : ObjectGraphType<OrderModel>
    {
        public OrderGraphType()
        {
            Field(x => x.Id);
            Field<ListGraphType<StringGraphType>>(nameof(OrderModel.Errors));
            Field(x => x.CreationDate);
            Field<ListGraphType<OrderItemGraphType>>(nameof(OrderModel.OrderItems));
            Field<OrderStatusEnumGraphType>(nameof(OrderModel.Status));
            Field<UserGraphType>(nameof(OrderModel.User));
        }
    }
}
