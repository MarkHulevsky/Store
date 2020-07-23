using Store.BuisnessLogicLayer.Models.Filters;
using Store.DataAccessLayer.Filters;
using static Store.DataAccessLayer.Entities.Enums.Enums;

namespace Store.BuisnessLogic.Helpers.Mappers.RequestFilterMappers
{
    public static class AuthorRequestFilterMapper
    {
        private static readonly Mapper<PagingModel, Paging> _pagingMapper = new Mapper<PagingModel, Paging>();
        public static AuthorRequestFilter Map(AuthorRequestFilterModel filterModel)
        {
            var filter = new AuthorRequestFilter();
            filter.Paging = _pagingMapper.Map(new Paging(), filterModel.Paging);
            filter.PropName = filterModel.PropName;
            filter.SortType = (SortType)filterModel.SortType;
            return filter;
        }
    }
}
