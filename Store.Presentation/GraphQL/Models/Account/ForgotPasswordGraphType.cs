using GraphQL.Types;
using Store.BuisnessLogic.Models.Account;

namespace Store.Presentation.GraphQL.Models.Account
{
    public class ForgotPasswordGraphType : ObjectGraphType<ForgotPasswordModel>
    {
        public ForgotPasswordGraphType()
        {
            Field(x => x.Email);
        }
    }
}
