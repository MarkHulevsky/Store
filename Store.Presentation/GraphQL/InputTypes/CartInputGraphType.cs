using GraphQL.Types;
using Store.BuisnessLogic.Models.Orders;

namespace Store.Presentation.GraphQL.InputTypes
{
    public class CartInputGraphType: ObjectGraphType<CartModel>
    {
        public CartInputGraphType()
        {
            Field(x => x.Id, nullable: true);
            Field(x => x.CreationDate, nullable: true);
            Field<ListGraphType<StringGraphType>>(nameof(CartModel.Errors));
            Field(x => x.UserId);
            Field<OrderInputGraphType>(nameof(CartModel.Order));
        }
    }
}
