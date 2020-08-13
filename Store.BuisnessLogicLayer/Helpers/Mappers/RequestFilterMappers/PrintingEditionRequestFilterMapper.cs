using Store.BuisnessLogicLayer.Models.Filters;
using Store.DataAccessLayer.Filters;
using static Store.DataAccessLayer.Entities.Enums.Enums;

namespace Store.BuisnessLogic.Helpers.Mappers.RequestFilterMappers
{
    public static class PrintingEditionRequestFilterMapper
    {
        private static readonly Mapper<PagingModel, Paging> _pagingMapper = new Mapper<PagingModel, Paging>();
        public static PrintingEditionsRequestFilter Map(PrintingEditionsRequestFilterModel filterModel)
        {
            var filter = new PrintingEditionsRequestFilter();
            filter.MaxPrice = filterModel.MaxPrice;
            filter.MinPrice = filterModel.MinPrice;
            filter.SearchString = filterModel.SearchString;
            filter.SortType = (SortType)filterModel.SortType;
            filter.Paging = _pagingMapper.Map(new Paging(), filterModel.Paging);

            foreach (var type in filterModel.Types)
            {
                filter.Types.Add((PrintingEditionType)type);
            }
            return filter;
        }
    }
}
