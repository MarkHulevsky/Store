using GraphQL.Types;
using Store.BuisnessLogic.Models.Account;

namespace Store.BuisnessLogic.GraphQL.Models.Account
{
    public class RegisterType: ObjectGraphType<RegisterModel>
    {
        public RegisterType()
        {
            Field(x => x.Email);
            Field(x => x.FirstName);
            Field(x => x.LastName);
            Field(x => x.Password);
            Field(x => x.ConfirmPassword);
        }
    }
}
