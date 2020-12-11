using GraphQL.Types;
using Store.BuisnessLogic.Models.Account;

namespace Store.Presentation.GraphQL.InputTypes
{
    public class LoginInputGraphType: InputObjectGraphType<LoginModel>
    {
        public LoginInputGraphType()
        {
            Field(x => x.Email);
            Field(x => x.Password);
        }
    }
}
