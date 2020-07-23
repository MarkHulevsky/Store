using Store.DataAccess.Filters.ResponseFilters;
using Store.DataAccessLayer.Entities;
using System.Collections.Generic;

namespace Store.DataAccess.Filters.ResponseFulters
{
    public class AuthorResponseFilter: BaseResponseFilter
    {
        public IEnumerable<Author> Authors { get; set; } = new List<Author>();
    }
}
