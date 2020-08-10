using Store.BuisnessLogic.Models.Filters;
using System.Collections.Generic;
using static Store.BuisnessLogicLayer.Models.Enums.Enums;

namespace Store.BuisnessLogicLayer.Models.Filters
{
    public class PrintingEditionsRequestFilterModel: BaseFilterModel
    {
        public IEnumerable<PrintingEditionType> Types { get; set; }
        public float MinPrice { get; set; }
        public float MaxPrice { get; set; }
        public string SearchString { get; set; }
        public PrintingEditionsRequestFilterModel()
        {
            Paging = new PagingModel
            {
                ItemsCount = 6
            };
        }
    }
}
