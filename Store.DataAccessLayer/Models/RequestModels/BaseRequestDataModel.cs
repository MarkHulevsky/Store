using static Shared.Enums.Enums;

namespace Store.DataAccess.Filters.RequestFilters
{
    public class BaseRequetDataModel
    {
        public SortType SortType { get; set; }
        public Paging Paging { get; set; }
    }
}
