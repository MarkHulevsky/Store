using Store.DataAccessLayer.Filters;
using static Store.DataAccessLayer.Entities.Enums.Enums;

namespace Store.DataAccess.Filters.RequestFilters
{
    public class BaseRequestFilter
    {
        public SortType SortType { get; set; }
        public Paging Paging { get; set; }
    }
}
