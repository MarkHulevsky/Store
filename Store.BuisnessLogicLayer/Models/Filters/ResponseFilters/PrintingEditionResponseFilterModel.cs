using Store.BuisnessLogicLayer.Models.PrintingEditions;
using System.Collections.Generic;

namespace Store.BuisnessLogic.Models.Filters.ResponseFilters
{
    public class PrintingEditionResponseFilterModel: BaseResponseFilter
    {
        public List<PrintingEditionModel> PrintingEditions { get; set; } = new List<PrintingEditionModel>();
    }
}
