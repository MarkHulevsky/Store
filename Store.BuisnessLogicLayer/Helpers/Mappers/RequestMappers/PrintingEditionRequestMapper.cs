using Store.BuisnessLogic.Models.Filters;
using Store.DataAccess.Filters;

namespace Store.BuisnessLogic.Helpers.Mappers.RequestFilterMappers
{
    public static class PrintingEditionRequestMapper
    {
        private static readonly Mapper<PagingModel, Paging> _pagingMapper;
        static PrintingEditionRequestMapper()
        {
            _pagingMapper = new Mapper<PagingModel, Paging>();
        }
        public static PrintingEditionsRequestDataModel Map(PrintingEditionsRequestModel filterModel)
        {
            var filter = new PrintingEditionsRequestDataModel
            {
                MaxPrice = filterModel.MaxPrice,
                MinPrice = filterModel.MinPrice,
                SearchString = filterModel.SearchString,
                SortType = filterModel.SortType,
                Paging = _pagingMapper.Map(filterModel.Paging)
            };

            foreach (var type in filterModel.Types)
            {
                filter.Types.Add(type);
            }
            return filter;
        }
    }
}
