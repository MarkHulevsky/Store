using GraphQL.Types;
using Store.BuisnessLogic.GraphQL.Models.User;
using Store.BuisnessLogic.Models.Filters.ResponseFilters;

namespace Store.BuisnessLogic.GraphQL.Models.Filters.ResponseModels
{
    public class UserResponseType: ObjectGraphType<UserResponseModel>
    {
        public UserResponseType()
        {
            Field(x => x.TotalCount);
            Field<ListGraphType<UserType>>(nameof(UserResponseModel.Users));
        }
    }
}
