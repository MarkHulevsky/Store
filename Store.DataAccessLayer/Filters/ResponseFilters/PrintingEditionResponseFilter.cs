using Store.DataAccess.Filters.ResponseFilters;
using Store.DataAccessLayer.Entities;
using System.Collections.Generic;

namespace Store.DataAccess.Filters.ResponseFulters
{
    public class PrintingEditionResponseFilter : BaseResponseFilter
    {
        public IEnumerable<PrintingEdition> PrintingEditions { get; set; } = new List<PrintingEdition>();
    }
}
