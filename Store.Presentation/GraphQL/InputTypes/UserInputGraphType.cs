using GraphQL.Types;
using Store.BuisnessLogic.Models.Users;

namespace Store.Presentation.GraphQL.InputTypes
{
    public class UserInputGraphType: InputObjectGraphType<UserModel>
    {
        public UserInputGraphType()
        {
            Field(x => x.Id);
            Field<ListGraphType<StringGraphType>>(nameof(UserModel.Errors));
            Field(x => x.CreationDate);
            Field(x => x.Email);
            Field(x => x.EmailConfirmed);
            Field(x => x.FirstName);
            Field(x => x.IsActive);
            Field(x => x.LastName);
            Field(x => x.Password);
            Field<ListGraphType<StringGraphType>>(nameof(UserModel.Roles));
        }
    }
}
