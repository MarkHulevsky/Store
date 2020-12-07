using GraphQL.Types;
using Store.BuisnessLogic.Models.Filters.ResponseFilters;
using Store.Presentation.GraphQL.Models.Orders;

namespace Store.Presentation.GraphQL.Models.Filters.ResponseModels
{
    public class OrderResponseGraphType : ObjectGraphType<OrderResponseModel>
    {
        public OrderResponseGraphType()
        {
            Field(x => x.TotalCount);
            Field<ListGraphType<OrderGraphType>>(nameof(OrderResponseModel.Orders));
        }
    }
}
