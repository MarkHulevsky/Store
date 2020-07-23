using Store.DataAccess.Filters.RequestFilters;

namespace Store.DataAccessLayer.Filters
{
    public class AuthorRequestFilter: BaseRequestFilter
    {
        public string PropName { get; set; }
        public AuthorRequestFilter()
        {
            Paging = new Paging
            {
                ItemsCount = 15,
                CurrentPage = 0
            };
        }
    }
}
