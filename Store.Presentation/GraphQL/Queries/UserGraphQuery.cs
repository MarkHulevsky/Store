using GraphQL;
using GraphQL.Types;
using Store.BuisnessLogic.Models.Filters;
using Store.BuisnessLogic.Services.Interfaces;
using Store.Presentation.GraphQL.InputTypes.RequestModels;
using Store.Presentation.GraphQL.Models.User;

namespace Store.Presentation.GraphQL.Queries
{
    public class UserGraphQuery: ObjectGraphType
    {
        public UserGraphQuery(IUserService userService)
        {
            FieldAsync<UserGraphType>("getProfile",
                resolve: async (context) =>
                {
                    return await userService.GetCurrentAsync();
                });

            FieldAsync<ListGraphType<UserGraphType>>("filter",
                arguments: new QueryArguments(new QueryArgument<UserRequestGraphType> { Name = "filter" }),
                resolve: async (context) =>
                {
                    var filter = context.GetArgument<UserRequestModel>("filter");
                    return await userService.FilterAsync(filter);
                });
        }
    }
}
