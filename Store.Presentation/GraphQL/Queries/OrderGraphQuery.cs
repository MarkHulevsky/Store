using GraphQL;
using GraphQL.Types;
using Store.BuisnessLogic.Models.Filters;
using Store.BuisnessLogic.Services.Interfaces;
using Store.Presentation.GraphQL.InputTypes.RequestModels;
using Store.Presentation.GraphQL.Models.Orders;

namespace Store.Presentation.GraphQL.Queries
{
    public class OrderGraphQuery: ObjectGraphType
    {
        public OrderGraphQuery(IOrderService orderService)
        {
            FieldAsync<ListGraphType<OrderGraphType>>("getUserOrders",
                resolve: async (context) =>
                {
                    return await orderService.GetUserOrdersAsync();
                });

            FieldAsync<ListGraphType<OrderGraphType>>("filter",
                arguments: new QueryArguments(new QueryArgument<OrderRequestGraphType> { Name = "filter" }),
                resolve: async (context) =>
                {
                    var filter = context.GetArgument<OrderRequestModel>("filter");
                    return await orderService.FilterAsync(filter);
                });
        }
    }
}
