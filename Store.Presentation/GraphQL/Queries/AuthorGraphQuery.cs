using GraphQL;
using GraphQL.Types;
using Store.BuisnessLogic.Models.Filters;
using Store.BuisnessLogic.Services.Interfaces;
using Store.Presentation.GraphQL.InputTypes.RequestModels;
using Store.Presentation.GraphQL.Models.Author;
using Store.Presentation.GraphQL.Models.Filters.ResponseModels;

namespace Store.Presentation.GraphQL.Queries
{
    public class AuthorGraphQuery : ObjectGraphType
    {
        private readonly IAuthorService _authorService;
        public AuthorGraphQuery(IAuthorService authorService)
        {
            _authorService = authorService;

            FieldAsync<ListGraphType<AuthorGraphType>>("allAuthors",
                resolve: async (context) => await _authorService.GetAllAsync());

            FieldAsync<AuthorResponseGraphType>("authors",
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

            
        }
    }
}
