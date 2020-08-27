using static Shared.Enums.Enums;

namespace Store.BuisnessLogic.Models.Filters
{
    public abstract class BaseRequestModel
    {
        public SortType SortType { get; set; }
        public PagingModel Paging { get; set; }
    }
}
