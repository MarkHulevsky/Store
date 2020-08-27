using Store.DataAccess.Entities;
using Store.DataAccess.Filters.ResponseFilters;
using System.Collections.Generic;

namespace Store.DataAccess.Filters.ResponseFulters
{
    public class PrintingEditionResponseDataModel : BaseResponseDataModel
    {
        public List<PrintingEdition> PrintingEditions { get; set; }
        public PrintingEditionResponseDataModel()
        {
            PrintingEditions = new List<PrintingEdition>();
        }
    }
}
