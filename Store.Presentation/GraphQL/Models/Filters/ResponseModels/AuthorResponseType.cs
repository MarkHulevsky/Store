using GraphQL.Types;
using Store.BuisnessLogic.Models.Filters.ResponseFilters;
using Store.Presentation.GraphQL.Models.Author;

namespace Store.Presentation.GraphQL.Models.Filters.ResponseModels
{
    public class AuthorResponseType : ObjectGraphType<AuthorResponseModel>
    {
        public AuthorResponseType()
        {
            Field<IntGraphType>(nameof(AuthorResponseModel.TotalCount));
            Field<ListGraphType<AuthorType>>(nameof(AuthorResponseModel.Authors));
        }
    }
}
