using GraphQL.Types;
using Store.BuisnessLogic.Models.Filters.ResponseFilters;
using Store.Presentation.GraphQL.Models.Orders;

namespace Store.Presentation.GraphQL.Models.Filters.ResponseModels
{
    public class OrderResponseType : ObjectGraphType<OrderResponseModel>
    {
        public OrderResponseType()
        {
            Field(x => x.TotalCount);
            Field<ListGraphType<OrderType>>(nameof(OrderResponseModel.Orders));
        }
    }
}
