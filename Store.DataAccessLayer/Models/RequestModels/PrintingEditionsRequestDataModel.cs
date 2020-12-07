using Store.DataAccess.Filters.RequestFilters;
using System.Collections.Generic;
using static Shared.Enums.Enums;

namespace Store.DataAccess.Filters
{
    public class PrintingEditionsRequestDataModel : BaseRequetDataModel
    {
        public List<PrintingEditionType> Types { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public string SearchString { get; set; }
        public PrintingEditionsRequestDataModel()
        {
            Types = new List<PrintingEditionType>();
        }
    }
}
