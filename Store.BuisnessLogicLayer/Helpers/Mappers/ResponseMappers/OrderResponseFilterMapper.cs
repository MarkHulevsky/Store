using Store.BuisnessLogic.Models.Filters.ResponseFilters;
using Store.BuisnessLogic.Models.Orders;
using Store.BuisnessLogic.Models.PrintingEditions;
using Store.BuisnessLogic.Models.Users;
using Store.DataAccess.Entities;
using Store.DataAccess.Filters.ResponseFulters;
using System.Collections.Generic;

namespace Store.BuisnessLogic.Helpers.Mappers.ResponseFilterMappers
{
    public static class OrderResponseFilterMapper
    {
        private static readonly Mapper<Order, OrderModel> _orderModelMapper = new Mapper<Order, OrderModel>();
        private static readonly Mapper<OrderItem, OrderItemModel> _orderItemModelMapper =
            new Mapper<OrderItem, OrderItemModel>();
        private static readonly Mapper<User, UserModel> _userModelMapper = new Mapper<User, UserModel>();
        private static readonly Mapper<PrintingEdition, PrintingEditionModel> _printingEditionModelMapper =
            new Mapper<PrintingEdition, PrintingEditionModel>();

        public static OrderResponseModel Map(OrderResponseDataModel responseFilter)
        {
            var responseFilterModel = new OrderResponseModel
            {
                TotalCount = responseFilter.TotalCount
            };
            foreach (var order in responseFilter.Orders)
            {
                var orderItemModels = new List<OrderItemModel>();
                foreach (var orderItem in order.OrderItems)
                {
                    var orderItemModel = _orderItemModelMapper.Map(orderItem);
                    orderItemModel.PrintingEdition = _printingEditionModelMapper.Map(orderItem.PrintingEdition);
                    orderItemModels.Add(orderItemModel);
                }
                var orderModel = _orderModelMapper.Map(order);
                orderModel.User = _userModelMapper.Map(order.User);
                orderModel.OrderItems = orderItemModels;
                responseFilterModel.Orders.Add(orderModel);
            }

            return responseFilterModel;
        }
    }
}
