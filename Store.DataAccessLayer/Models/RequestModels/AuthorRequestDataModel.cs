using Store.DataAccess.Filters.RequestFilters;

namespace Store.DataAccess.Filters
{
    public class AuthorRequestDataModel : BaseRequetDataModel
    {
        public string SortPropertyName { get; set; }
    }
}
