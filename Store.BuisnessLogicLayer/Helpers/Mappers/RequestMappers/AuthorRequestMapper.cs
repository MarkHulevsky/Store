using Store.BuisnessLogic.Models.Filters;
using Store.DataAccess.Filters;

namespace Store.BuisnessLogic.Helpers.Mappers.RequestFilterMappers
{
    public static class AuthorRequestMapper
    {
        private static readonly Mapper<PagingModel, Paging> _pagingMapper;
        static AuthorRequestMapper()
        {
            _pagingMapper = new Mapper<PagingModel, Paging>();
        }
        public static AuthorRequestDataModel Map(AuthorRequestModel filterModel)
        {
            var filter = new AuthorRequestDataModel
            {
                Paging = _pagingMapper.Map(filterModel.Paging),
                SortPropertyName = filterModel.SortPropertyName,
                SortType = filterModel.SortType
            };
            return filter;
        }
    }
}
