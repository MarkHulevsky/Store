using GraphQL;
using GraphQL.Types;
using Store.BuisnessLogic.Models.Orders;
using Store.BuisnessLogic.Models.Payments;
using Store.BuisnessLogic.Services.Interfaces;
using Store.Presentation.GraphQL.InputTypes;
using Store.Presentation.GraphQL.Models.Orders;

namespace Store.Presentation.GraphQL.Mutations
{
    public class OrderGraphMutation: ObjectGraphType
    {
        public OrderGraphMutation(IOrderService orderService)
        {
            FieldAsync<OrderGraphType>("add",
                arguments: new QueryArguments(new QueryArgument<CartInputGraphType> { Name = "cartModel" }),
                resolve: async (context) =>
                {
                    var cartModel = context.GetArgument<CartModel>("cartModel");
                    return await orderService.CreateAsync(cartModel);
                });

            FieldAsync<StringGraphType>("remove",
                arguments: new QueryArguments(new QueryArgument<StringGraphType> { Name = "id" }),
                resolve: async (context) =>
                {
                    var id = context.GetArgument<string>("id");
                    return await orderService.RemoveAsync(id);
                });

            FieldAsync<ListGraphType<StringGraphType>>("pay",
                arguments: new QueryArguments(new QueryArgument<PaymentInputGraphType> { Name = "paymentModel" }),
                resolve: async (context) =>
                {
                    var paymentModel = context.GetArgument<PaymentModel>("paymentModel");
                    return await orderService.PayOrderAsync(paymentModel);
                });
        }
    }
}
