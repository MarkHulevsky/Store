using GraphQL.Types;
using Store.BuisnessLogic.Models.Account;

namespace Store.Presentation.GraphQL.Models.Account
{
    public class RegisterGraphType : ObjectGraphType<RegisterModel>
    {
        public RegisterGraphType()
        {
            Field(x => x.Email);
            Field(x => x.FirstName);
            Field(x => x.LastName);
            Field(x => x.Password);
            Field(x => x.ConfirmPassword);
        }
    }
}
