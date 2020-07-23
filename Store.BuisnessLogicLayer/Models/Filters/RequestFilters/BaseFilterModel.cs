using Store.BuisnessLogicLayer.Models.Filters;
using static Store.BuisnessLogicLayer.Models.Enums.Enums;

namespace Store.BuisnessLogic.Models.Filters
{
    public abstract class BaseFilterModel
    {
        public SortType SortType { get; set; }
        public PagingModel Paging { get; set; }
    }
}
