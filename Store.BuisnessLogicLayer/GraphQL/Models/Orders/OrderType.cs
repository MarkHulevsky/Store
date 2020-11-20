using GraphQL.Types;
using Store.BuisnessLogic.GraphQL.Enums;
using Store.BuisnessLogic.GraphQL.Models.User;
using Store.BuisnessLogic.Models.Orders;

namespace Store.BuisnessLogic.GraphQL.Models.Orders
{
    public class OrderType: ObjectGraphType<OrderModel>
    {
        public OrderType()
        {
            Field(x => x.Id);
            Field<ListGraphType<StringGraphType>>(nameof(OrderModel.Errors));
            Field(x => x.CreationDate);
            Field<ListGraphType<OrderItemType>>(nameof(OrderModel.OrderItems));
            Field<OrderStatusGraphType>(nameof(OrderModel.Status));
            Field<UserType>(nameof(OrderModel.User));
        }
    }
}
