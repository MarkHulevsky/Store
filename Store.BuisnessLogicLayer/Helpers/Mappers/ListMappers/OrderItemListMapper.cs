using Store.BuisnessLogic.Models.Orders;
using Store.DataAccess.Entities;
using System;
using System.Collections.Generic;

namespace Store.BuisnessLogic.Helpers.Mappers.ListMappers
{
    public static class OrderItemListMapper
    {
        private static readonly Mapper<OrderItemModel, OrderItem> _orderItemMapper;
        static OrderItemListMapper()
        {
            _orderItemMapper = new Mapper<OrderItemModel, OrderItem>();
        }
        public static List<OrderItem> Map(List<OrderItemModel> orderItemModels, Guid orderId)
        {
            var orderItems = new List<OrderItem>();
            foreach (var orderItemModel in orderItemModels)
            {
                var orderItem = _orderItemMapper.Map(orderItemModel);
                orderItem.OrderId = orderId;
                orderItems.Add(orderItem);
            }
            return orderItems;
        }
    }
}
