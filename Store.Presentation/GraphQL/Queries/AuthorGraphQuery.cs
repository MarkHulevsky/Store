using GraphQL;
using GraphQL.Types;
using Store.BuisnessLogic.Models.Filters;
using Store.BuisnessLogic.Services.Interfaces;
using Store.Presentation.GraphQL.Models.Author;
using Store.Presentation.GraphQL.Models.Filters.RequestModels;
using Store.Presentation.GraphQL.Models.Filters.ResponseModels;

namespace Store.Presentation.GraphQL.Queries
{
    public class AuthorGraphQuery : ObjectGraphType
    {
        private readonly IAuthorService _authorService;
        public AuthorGraphQuery(IAuthorService authorService)
        {
            _authorService = authorService;

            FieldAsync<ListGraphType<AuthorType>>("allAuthors",
                resolve: async (context) => await _authorService.GetAllAsync());

            FieldAsync<AuthorResponseType>("authors",
                arguments: new QueryArguments(new QueryArgument<AuthorRequestGraphType> 
                {
                    Name = "filter"
                }),
                resolve: async (context) =>
                {
                    var filter = context.GetArgument<AuthorRequestModel>("filter");
                    var result = await _authorService.FilterAsync(filter);
                    return result;
                });

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
        }
    }
}
