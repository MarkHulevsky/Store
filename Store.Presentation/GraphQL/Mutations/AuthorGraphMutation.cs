using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Store.BuisnessLogic.Models.Authors;
using Store.BuisnessLogic.Services.Interfaces;
using Store.Presentation.GraphQL.InputTypes;
using Store.Presentation.GraphQL.Models.Author;

namespace Store.Presentation.GraphQL.Mutations
{
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AuthorGraphMutation: ObjectGraphType
    {
        private readonly IAuthorService _authorService;
        public AuthorGraphMutation(IAuthorService authorService)
        {
            _authorService = authorService;

            FieldAsync<StringGraphType>("removeAuthor",
                arguments: new QueryArguments(new QueryArgument<StringGraphType>
                {
                    Name = "id"
                }),
                resolve: async (context) =>
                {
                    var id = context.GetArgument<string>("id");
                    await _authorService.RemoveAsync(id);
                    return id;
                });

            FieldAsync<AuthorGraphType>("editAuthor",
                arguments: new QueryArguments(new QueryArgument<AuthorInputGraphType>
                {
                    Name = "author"
                }),
                resolve: async (context) =>
                {
                    var author = context.GetArgument<AuthorModel>("author");
                    return await _authorService.EditAsync(author);
                });

            FieldAsync<AuthorGraphType>("addAuthor",
                arguments: new QueryArguments(new QueryArgument<AuthorInputGraphType>
                {
                    Name = "author"
                }),
                resolve: async (context) =>
                {
                    var author = context.GetArgument<AuthorModel>("author");
                    return await _authorService.CreateAsync(author);
                });
        }
    }
}
