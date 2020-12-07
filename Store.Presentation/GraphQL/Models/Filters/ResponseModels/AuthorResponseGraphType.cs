using GraphQL.Types;
using Store.BuisnessLogic.Models.Filters.ResponseFilters;
using Store.Presentation.GraphQL.Models.Author;

namespace Store.Presentation.GraphQL.Models.Filters.ResponseModels
{
    public class AuthorResponseGraphType : ObjectGraphType<AuthorResponseModel>
    {
        public AuthorResponseGraphType()
        {
            Field<IntGraphType>(nameof(AuthorResponseModel.TotalCount));
            Field<ListGraphType<AuthorGraphType>>(nameof(AuthorResponseModel.Authors));
        }
    }
}
