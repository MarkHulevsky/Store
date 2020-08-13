using Store.DataAccess.Filters.RequestFilters;

namespace Store.DataAccessLayer.Filters
{
    public class AuthorRequestFilter : BaseRequestFilter
    {
        public string PropName { get; set; }
    }
}
