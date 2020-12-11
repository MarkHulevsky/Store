using GraphQL;
using GraphQL.Types;
using Store.BuisnessLogic.Models.Users;
using Store.BuisnessLogic.Services.Interfaces;
using Store.Presentation.GraphQL.InputTypes;
using Store.Presentation.GraphQL.Models.User;

namespace Store.Presentation.GraphQL.Mutations
{
    public class UserGraphMutation: ObjectGraphType
    {
        public UserGraphMutation(IUserService userService)
        {
            FieldAsync<UserGraphType>("changeStatus",
                arguments: new QueryArguments(new QueryArgument<UserInputGraphType> { Name = "user" }),
                resolve: async (context) =>
                {
                    var user = context.GetArgument<UserModel>("user");
                    await userService.ChangeStatusAsync(user);
                    return user;
                });

            FieldAsync<ListGraphType<StringGraphType>>("remove",
                arguments: new QueryArguments(new QueryArgument<StringGraphType> { Name = "id" }),
                resolve: async (context) =>
                {
                    var id = context.GetArgument<string>("id");
                    return await userService.RemoveAsync(id);
                });

            FieldAsync<ListGraphType<StringGraphType>>("edit",
                arguments: new QueryArguments(new QueryArgument<EditProfileInputGraphType> { Name = "editProfile" }),
                resolve: async (context) =>
                {
                    var editProfile = context.GetArgument<EditProfileModel>("editProfile");
                    return await userService.EditAsync(editProfile);
                });
        }
    }
}
