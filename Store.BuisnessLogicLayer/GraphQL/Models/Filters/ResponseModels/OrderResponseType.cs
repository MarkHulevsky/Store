using GraphQL.Types;
using Store.BuisnessLogic.GraphQL.Models.Orders;
using Store.BuisnessLogic.Models.Filters.ResponseFilters;

namespace Store.BuisnessLogic.GraphQL.Models.Filters.ResponseModels
{
    public class OrderResponseType: ObjectGraphType<OrderResponseModel>
    {
        public OrderResponseType()
        {
            Field(x => x.TotalCount);
            Field<ListGraphType<OrderType>>(nameof(OrderResponseModel.Orders));
        }
    }
}
