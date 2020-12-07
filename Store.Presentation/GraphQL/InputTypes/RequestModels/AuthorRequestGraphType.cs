using GraphQL.Types;
using Store.BuisnessLogic.Models.Filters;
using Store.Presentation.GraphQL.Models.Filters;

namespace Store.Presentation.GraphQL.InputTypes.RequestModels
{
    class AuthorRequestGraphType : InputObjectGraphType<AuthorRequestModel>
    {
        public AuthorRequestGraphType()
        {
            Field(x => x.SortPropertyName);
            Field<IntGraphType>(nameof(AuthorRequestModel.SortType), resolve: context => (int)context.Source.SortType);
            Field<PagingGraphType>(nameof(AuthorRequestModel.Paging));
        }
    }
}
