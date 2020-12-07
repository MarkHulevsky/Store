using GraphQL.Types;
using Store.BuisnessLogic.Models.Filters.ResponseFilters;
using Store.Presentation.GraphQL.Models.User;

namespace Store.Presentation.GraphQL.Models.Filters.ResponseModels
{
    public class UserResponseType : ObjectGraphType<UserResponseModel>
    {
        public UserResponseType()
        {
            Field(x => x.TotalCount);
            Field<ListGraphType<UserType>>(nameof(UserResponseModel.Users));
        }
    }
}
