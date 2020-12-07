using GraphQL.Types;
using Store.BuisnessLogic.Models.Orders;

namespace Store.Presentation.GraphQL.Models.Orders
{
    public class CartGraphType : ObjectGraphType<CartModel>
    {
        public CartGraphType()
        {
            Field(x => x.Id);
            Field(x => x.CreationDate);
            Field<ListGraphType<StringGraphType>>(nameof(CartModel.Errors));
            Field(x => x.UserId);
            Field<OrderGraphType>(nameof(CartModel.Order));
        }
    }
}
