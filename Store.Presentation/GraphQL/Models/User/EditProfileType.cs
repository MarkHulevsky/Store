using GraphQL.Types;
using Store.BuisnessLogic.Models.Users;

namespace Store.Presentation.GraphQL.Models.User
{
    public class EditProfileType : ObjectGraphType<EditProfileModel>
    {
        public EditProfileType()
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
