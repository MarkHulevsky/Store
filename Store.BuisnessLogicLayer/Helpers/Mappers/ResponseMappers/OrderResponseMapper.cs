using Store.BuisnessLogic.Models.Filters.ResponseFilters;
using Store.BuisnessLogic.Models.Orders;
using Store.BuisnessLogic.Models.PrintingEditions;
using Store.BuisnessLogic.Models.Users;
using Store.DataAccess.Entities;
using Store.DataAccess.Filters.ResponseFulters;
using System.Collections.Generic;

namespace Store.BuisnessLogic.Helpers.Mappers.ResponseFilterMappers
{
    public static class OrderResponseMapper

    {
        private static readonly Mapper<Order, OrderModel> _orderModelMapper;
        private static readonly Mapper<OrderItem, OrderItemModel> _orderItemModelMapper;
        private static readonly Mapper<User, UserModel> _userModelMapper;
        private static readonly Mapper<PrintingEdition, PrintingEditionModel> _printingEditionModelMapper;

        static OrderResponseMapper()
        {
            _orderModelMapper = new Mapper<Order, OrderModel>();
            _orderItemModelMapper = new Mapper<OrderItem, OrderItemModel>();
            _userModelMapper = new Mapper<User, UserModel>();
            _printingEditionModelMapper = new Mapper<PrintingEdition, PrintingEditionModel>();
        }

        public static OrderResponseModel Map(OrderResponseDataModel responseFilter)
        {
            if (responseFilter is null)
            {
                return new OrderResponseModel();
            }

            var responseFilterModel = new OrderResponseModel
            {
                TotalCount = responseFilter.TotalCount
            };
            foreach (var order in responseFilter.Orders)
            {
                var orderItemModels = new List<OrderItemModel>();
                foreach (var orderItem in order.OrderItems)
                {
                    if (orderItem == null)
                    {
                        continue;
                    }
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
