using System.Collections.Generic;
using static Shared.Enums.Enums;

namespace Store.BuisnessLogic.Models.Filters
{
    public class PrintingEditionsRequestModel : BaseRequestModel
    {
        public IEnumerable<PrintingEditionType> Types { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public string SearchString { get; set; }
        public PrintingEditionsRequestModel()
        {
            Types = new List<PrintingEditionType>();
        }
    }
}
