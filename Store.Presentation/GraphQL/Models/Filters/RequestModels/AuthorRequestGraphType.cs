using GraphQL.Types;
using Store.BuisnessLogic.Models.Filters;

namespace Store.Presentation.GraphQL.Models.Filters.RequestModels
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
