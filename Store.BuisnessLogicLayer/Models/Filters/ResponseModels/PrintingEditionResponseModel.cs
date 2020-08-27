using Store.BuisnessLogic.Models.PrintingEditions;
using System.Collections.Generic;

namespace Store.BuisnessLogic.Models.Filters.ResponseFilters
{
    public class PrintingEditionResponseModel : BaseResponseModel
    {
        public List<PrintingEditionModel> PrintingEditions { get; set; }
        public PrintingEditionResponseModel()
        {
            PrintingEditions = new List<PrintingEditionModel>();
        }
    }
}
