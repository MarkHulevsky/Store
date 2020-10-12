using Store.BuisnessLogic.Models.Orders;
using Store.BuisnessLogic.Models.PrintingEditions;
using Store.DataAccess.Entities;
using System.Collections.Generic;

namespace Store.BuisnessLogic.Helpers.Mappers.ListMappers
{
    public static class OrderModelListMapper
    {
        private static readonly Mapper<OrderItem, OrderItemModel> _orderItemModelMapper;
        private static readonly Mapper<PrintingEdition, PrintingEditionModel> _printingEditionModelMapper;
        private static readonly Mapper<Order, OrderModel> _orderModelMapper;
        static OrderModelListMapper()
        {
            _orderItemModelMapper = new Mapper<OrderItem, OrderItemModel>();
            _printingEditionModelMapper = new Mapper<PrintingEdition, PrintingEditionModel>();
            _orderModelMapper = new Mapper<Order, OrderModel>();
        }
        public static List<OrderModel> Map(List<Order> orders)
        {
            var orderModels = new List<OrderModel>();
            foreach (var order in orders)
            {
                var orderItemModels = new List<OrderItemModel>();
                foreach (var orderItem in order.OrderItems)
                {
                    var orderItemModel = _orderItemModelMapper.Map(orderItem);
                    if (orderItem != null)
                    {
                        orderItemModel.PrintingEdition = _printingEditionModelMapper.Map(orderItem.PrintingEdition);
                        orderItemModels.Add(orderItemModel);
                    }
                }
                var orderModel = _orderModelMapper.Map(order);
                orderModel.Status = order.Status;
                orderModel.OrderItems = orderItemModels;
                orderModels.Add(orderModel);
            }
            return orderModels;
        }
    }
}
