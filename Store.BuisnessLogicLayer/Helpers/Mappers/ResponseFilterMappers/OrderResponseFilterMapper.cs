using Store.BuisnessLogic.Models.Filters.ResponseFilters;
using Store.BuisnessLogicLayer.Models.Orders;
using Store.DataAccess.Filters.ResponseFulters;
using Store.DataAccessLayer.Entities;

namespace Store.BuisnessLogic.Helpers.Mappers.ResponseFilterMappers
{
    public static class OrderResponseFilterMapper
    {
        private static readonly Mapper<Order, OrderModel> _orderModelMapper = new Mapper<Order, OrderModel>();

        public static OrderResponseFilterModel Map(OrderResponseFilter responseFilter)
        {
            var responseFilterModel = new OrderResponseFilterModel
            {
                TotalCount = responseFilter.TotalCount
            };
            foreach (var order in responseFilter.Orders)
            {
                var orderModel = _orderModelMapper.Map(new OrderModel(), order);
                responseFilterModel.Orders.Add(orderModel);
            }

            return responseFilterModel;
        }
    }
}
