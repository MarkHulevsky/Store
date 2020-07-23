using Store.BuisnessLogicLayer.Models.Filters;
using Store.DataAccessLayer.Filters;
using static Store.DataAccessLayer.Entities.Enums.Enums;

namespace Store.BuisnessLogic.Helpers.Mappers.RequestFilterMappers
{
    public static class OrderRequestFilterMapper
    {
        private static readonly Mapper<PagingModel, Paging> _pagingMapper = new Mapper<PagingModel, Paging>();
        public static OrderRequestFilter Map(OrderRequestFilterModel filterModel)
        {
            var filter = new OrderRequestFilter();
            filter.PropName = filterModel.PropName;
            filter.SortType = (SortType)filterModel.SortType;
            filter.Paging = _pagingMapper.Map(new Paging(), filterModel.Paging);
            foreach(var status in filterModel.OrderStatuses)
            {
                filter.OrderStatuses.Add((OrderStatus)status);
            }
            return filter;
        }
    }
}
