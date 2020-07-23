using Store.BuisnessLogicLayer.Models.Authors;
using System.Collections.Generic;

namespace Store.BuisnessLogic.Models.Filters.ResponseFilters
{
    public class AuthorResponseFilterModel: BaseResponseFilter
    {
        public List<AuthorModel> Authors { get; set; } = new List<AuthorModel>();
    }
}
