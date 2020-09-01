using Store.DataAccess.Entities;
using Store.DataAccess.Filters.ResponseFilters;
using System.Collections.Generic;

namespace Store.DataAccess.Filters.ResponseFulters
{
    public class AuthorResponseDataModel : BaseResponseDataModel
    {
        public IEnumerable<Author> Authors { get; set; }
        public AuthorResponseDataModel()
        {
            Authors = new List<Author>();
        }
    }
}
