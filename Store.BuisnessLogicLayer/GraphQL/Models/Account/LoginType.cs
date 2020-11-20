using GraphQL.Types;
using Store.BuisnessLogic.Models.Account;

namespace Store.BuisnessLogic.GraphQL.Models.Account
{
    public class LoginType: ObjectGraphType<LoginModel>
    {
        public LoginType()
        {
            Field(x => x.Email);
            Field(x => x.Password);
        }
    }
}
