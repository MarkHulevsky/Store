using Store.BuisnessLogic.Models.Authors;
using System.Collections.Generic;

namespace Store.BuisnessLogic.Models.Filters.ResponseFilters
{
    public class AuthorResponseModel : BaseResponseModel
    {
        public List<AuthorModel> Authors { get; set; }
        public AuthorResponseModel()
        {
            Authors = new List<AuthorModel>();
        }
    }
}
