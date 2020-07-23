using Store.BuisnessLogicLayer.Models.Filters;
using Store.DataAccessLayer.Filters;
using static Store.DataAccessLayer.Entities.Enums.Enums;

namespace Store.BuisnessLogic.Helpers.Mappers.RequestFilterMappers
{
    public static class UserRequestFilterMapper
    {
        private static readonly Mapper<PagingModel, Paging> _pagingMapper = new Mapper<PagingModel, Paging>();
        public static UserRequestFilter Map(UserRequestFilterModel filterModel)
        {
            var filter = new UserRequestFilter();
            filter.PropName = filterModel.PropName;
            filter.SearchString = filterModel.SearchString;
            filter.SortType = (SortType)filterModel.SortType;
            filter.Paging = _pagingMapper.Map(new Paging(), filterModel.Paging);
            foreach(var status in filterModel.Statuses)
            {
                filter.Statuses.Add(status);
            }
            return filter;
        }
    }
}
