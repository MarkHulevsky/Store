using GraphQL.Types;
using Store.BuisnessLogic.Models.Users;

namespace Store.Presentation.GraphQL.InputTypes
{
    public class EditProfileInputGraphType:  InputObjectGraphType<EditProfileModel>
    {
        public EditProfileInputGraphType()
        {
            Field(x => x.Id);
            Field(x => x.Email);
            Field(x => x.FirstName);
            Field(x => x.LastName);
            Field(x => x.Password);
            Field(x => x.ConfirmPassword);
        }
    }
}
