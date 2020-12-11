using GraphQL;
using GraphQL.Types;
using Store.BuisnessLogic.Helpers.Interfaces;
using Store.BuisnessLogic.Models.Account;
using Store.BuisnessLogic.Models.Token;
using Store.BuisnessLogic.Services.Interfaces;
using Store.Presentation.GraphQL.InputTypes;
using Store.Presentation.GraphQL.Models.Token;
using Store.Presentation.GraphQL.Models.User;

namespace Store.Presentation.GraphQL.Queries
{
    public class AccountGraphQuery: ObjectGraphType
    {
        public AccountGraphQuery(IAccountService accountService, IJwtProvider jwtProvider)
        {
            FieldAsync<ListGraphType<StringGraphType>>("forgotPassword",
                arguments: new QueryArguments(new QueryArgument<StringGraphType> { Name = "frogotPasswordModel" }),
                resolve: async (context) =>
                {
                    var forgotPasswordModel = context.GetArgument<ForgotPasswordModel>("frogotPasswordModel");
                    return await accountService.ResetPasswordAsync(forgotPasswordModel.Email);
                });

            Field<JwtTokenGraphType>("refreshToken",
                arguments: new QueryArguments(new QueryArgument<JwtTokenInputGraphType> { Name = "jwtToken" }),
                resolve: (context) =>
                {
                    var token = context.GetArgument<JwtTokenModel>("jwtToken");
                    return jwtProvider.RefreshToken(token);
                });

            FieldAsync<ListGraphType<StringGraphType>>("signUp",
                arguments: new QueryArguments(new QueryArgument<RegisterInputGraphType> { Name = "registerModel" }),
                resolve: async (context) =>
                {
                    var registerModel = context.GetArgument<RegisterModel>("registerModel");
                    return await accountService.RegisterAsync(registerModel);
                });

            FieldAsync<UserGraphType>("signIn",
                arguments: new QueryArguments(new QueryArgument<LoginInputGraphType> { Name = "loginModel" }),
                resolve: async (context) =>
                {
                    var loginModel = context.GetArgument<LoginModel>("loginModel");
                    return await accountService.LoginAsync(loginModel);
                });
        }
    }
}
