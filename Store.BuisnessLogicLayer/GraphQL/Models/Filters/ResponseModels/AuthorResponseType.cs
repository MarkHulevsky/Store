using GraphQL.Types;
using Store.BuisnessLogic.GraphQL.Models.Author;
using Store.BuisnessLogic.Models.Filters.ResponseFilters;

namespace Store.BuisnessLogic.GraphQL.Models.Filters.ResponseModels
{
    public class AuthorResponseType: ObjectGraphType<AuthorResponseModel>
    {
        public AuthorResponseType()
        {
            Field(x => x.TotalCount);
            Field<AuthorType>(nameof(AuthorResponseModel.Authors));
        }
    }
}
