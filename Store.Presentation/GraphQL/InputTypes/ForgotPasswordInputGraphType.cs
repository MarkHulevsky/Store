using GraphQL.Types;
using Store.BuisnessLogic.Models.Account;

namespace Store.Presentation.GraphQL.InputTypes
{
    public class ForgotPasswordInputGraphType: InputObjectGraphType<ForgotPasswordModel>
    {
        public ForgotPasswordInputGraphType()
        {
            Field(x => x.Email);
        }
    }
}
