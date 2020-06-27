using System.Collections.Generic;
using static Store.DataAccessLayer.Entities.Enums.Enums;

namespace Store.DataAccessLayer.Filters
{
    public class PrintingEditionsRequestFilter
    {
        public List<PrintingEditionType> Types { get; set; }
        public SortType SortType { get; set; }
        public float MinPrice { get; set; }
        public float MaxPrice { get; set; }
        public string SearchFilter { get; set; }
        public Currency Currency { get; set; }
        public Paging Paging { get; set; }
        public PrintingEditionsRequestFilter()
        {
            Paging = new Paging
            {
                ItemsCount = 6,
                Number = 0
            };
        }
    }
}
