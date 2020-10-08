using Store.BuisnessLogic.Models.Filters;
using Store.DataAccess.Filters;

namespace Store.BuisnessLogic.Helpers.Mappers.RequestFilterMappers
{
    public static class UserRequestMapper
    {
        private static readonly Mapper<PagingModel, Paging> _pagingMapper;
        static UserRequestMapper()
        {
            _pagingMapper = new Mapper<PagingModel, Paging>();
        }
        public static UserRequestDataModel Map(UserRequestModel filterModel)
        {
            var filter = new UserRequestDataModel
            {
                SortPropertyName = filterModel.SortPropertyName,
                SearchString = filterModel.SearchString,
                SortType = filterModel.SortType,
                Paging = _pagingMapper.Map(filterModel.Paging)
            };
            return filter;
        }
    }
}
