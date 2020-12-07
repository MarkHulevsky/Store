using GraphQL.Types;
using Store.BuisnessLogic.Models.Users;
using Store.Presentation.GraphQL.Models.Orders;

namespace Store.Presentation.GraphQL.Models.User
{
    public class UserType : ObjectGraphType<UserModel>
    {
        public UserType()
        {
            Field(x => x.Id);
            Field<ListGraphType<StringGraphType>>(nameof(UserModel.Errors));
            Field(x => x.CreationDate);
            Field(x => x.Email);
            Field(x => x.EmailConfirmed);
            Field(x => x.FirstName);
            Field(x => x.IsActive);
            Field(x => x.LastName);
            Field<OrderType>(nameof(UserModel.Orders));
            Field(x => x.Password);
            Field<ListGraphType<StringGraphType>>(nameof(UserModel.Roles));
        }
    }
}
