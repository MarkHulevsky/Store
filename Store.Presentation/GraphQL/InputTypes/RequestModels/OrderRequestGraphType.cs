using GraphQL.Types;
using Store.BuisnessLogic.Models.Filters;
using Store.Presentation.GraphQL.Models.Filters;

namespace Store.Presentation.GraphQL.InputTypes.RequestModels
{
    public class OrderRequestGraphType: InputObjectGraphType<OrderRequestModel>
    {
        public OrderRequestGraphType()
        {
            Field(x => x.SortPropertyName);
            Field<IntGraphType>(nameof(AuthorRequestModel.SortType), resolve: context => (int)context.Source.SortType);
            Field<PagingGraphType>(nameof(AuthorRequestModel.Paging));
        }
    }
}
