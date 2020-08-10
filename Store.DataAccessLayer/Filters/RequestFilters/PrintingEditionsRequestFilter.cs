using Store.DataAccess.Filters.RequestFilters;
using System.Collections.Generic;
using static Store.DataAccessLayer.Entities.Enums.Enums;

namespace Store.DataAccessLayer.Filters
{
    public class PrintingEditionsRequestFilter: BaseRequestFilter
    {
        public List<PrintingEditionType> Types { get; set; } = new List<PrintingEditionType>();
        public float MinPrice { get; set; }
        public float MaxPrice { get; set; }
        public string SearchString { get; set; }
        public PrintingEditionsRequestFilter()
        {
            Paging = new Paging
            {
                ItemsCount = 6,
                CurrentPage = 0
            };
        }
    }
}
