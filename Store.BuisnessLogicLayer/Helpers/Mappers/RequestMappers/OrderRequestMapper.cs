using Store.BuisnessLogic.Models.Filters;
using Store.DataAccess.Filters;

namespace Store.BuisnessLogic.Helpers.Mappers.RequestFilterMappers
{
    public static class OrderRequestMapper
    {
        private static readonly Mapper<PagingModel, Paging> _pagingMapper = new Mapper<PagingModel, Paging>();
        public static OrderRequestDataModel Map(OrderRequestModel filterModel)
        {
            var filter = new OrderRequestDataModel
            {
                SortPropertyName = filterModel.SortPropertyName,
                SortType = filterModel.SortType,
                Paging = _pagingMapper.Map(new Paging(), filterModel.Paging)
            };
            foreach (var status in filterModel.OrderStatuses)
            {
                filter.OrderStatuses.Add(status);
            }
            return filter;
        }
    }
}
