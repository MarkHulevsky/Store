using GraphQL.Types;
using Store.BuisnessLogic.Models.Filters.ResponseFilters;
using Store.Presentation.GraphQL.Models.User;

namespace Store.Presentation.GraphQL.Models.Filters.ResponseModels
{
    public class UserResponseGraphType : ObjectGraphType<UserResponseModel>
    {
        public UserResponseGraphType()
        {
            Field(x => x.TotalCount);
            Field<ListGraphType<UserGraphType>>(nameof(UserResponseModel.Users));
        }
    }
}
