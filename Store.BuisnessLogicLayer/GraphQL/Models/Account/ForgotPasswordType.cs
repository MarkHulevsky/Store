using GraphQL.Types;
using Store.BuisnessLogic.Models.Account;

namespace Store.BuisnessLogic.GraphQL.Models.Account
{
    public class ForgotPasswordType: ObjectGraphType<ForgotPasswordModel>
    {
        public ForgotPasswordType()
        {
            Field(x => x.Email);
        }
    }
}
