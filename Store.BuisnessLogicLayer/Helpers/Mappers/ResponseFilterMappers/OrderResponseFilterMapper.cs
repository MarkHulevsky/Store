using Store.BuisnessLogic.Models.Filters.ResponseFilters;
using Store.BuisnessLogicLayer.Models.Orders;
using Store.DataAccess.Filters.ResponseFulters;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Repositories.Interfaces;
using System.Collections.Generic;

namespace Store.BuisnessLogic.Helpers.Mappers.ResponseFilterMappers
{
    public static class OrderResponseFilterMapper
    {
        private static readonly Mapper<Order, OrderModel> _orderModelMapper = new Mapper<Order, OrderModel>();
        private static readonly Mapper<OrderItem, OrderItemModel> _orderItemModelMapper =
            new Mapper<OrderItem, OrderItemModel>();

        public static OrderResponseFilterModel Map(OrderResponseFilter responseFilter)
        {
            var responseFilterModel = new OrderResponseFilterModel();
            responseFilterModel.TotalCount = responseFilter.TotalCount;
            foreach(var order in responseFilter.Orders)
            {
                var orderModel = _orderModelMapper.Map(new OrderModel(), order);
                //var orderItemModels = GetOrderItemModels(order.OrderItems);
                //orderModel.OrderItems = orderItemModels;
                responseFilterModel.Orders.Add(orderModel);
            }

            return responseFilterModel;
        }
        //private static List<OrderItemModel> GetOrderItemModels(IEnumerable<OrderItem> orderItems)
        //{
        //    var orderItemModels = new List<OrderItemModel>();
        //    foreach (var orderItem in orderItems)
        //    {
        //        var orderItemModel = _orderItemModelMapper.Map(new OrderItemModel(), orderItem);
        //        orderItemModels.Add(orderItemModel);
        //    }
        //    return orderItemModels;
        //}
    }
}
