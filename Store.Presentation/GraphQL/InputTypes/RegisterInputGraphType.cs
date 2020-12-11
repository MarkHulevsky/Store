using GraphQL.Types;
using Store.BuisnessLogic.Models.Account;

namespace Store.Presentation.GraphQL.InputTypes
{
    public class RegisterInputGraphType: InputObjectGraphType<RegisterModel>
    {
        public RegisterInputGraphType()
        {
            Field(x => x.Email);
            Field(x => x.FirstName);
            Field(x => x.LastName);
            Field(x => x.Password);
            Field(x => x.ConfirmPassword);
        }
    }
}
