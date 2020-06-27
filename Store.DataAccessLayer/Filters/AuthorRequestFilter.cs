using System;
using System.Collections.Generic;
using System.Text;
using static Store.DataAccessLayer.Entities.Enums.Enums;

namespace Store.DataAccessLayer.Filters
{
    public class AuthorRequestFilter
    {
        public string PropName { get; set; }
        public SortType SortType { get; set; }
        public Paging Paging { get; set; }
        public AuthorRequestFilter()
        {
            Paging = new Paging
            {
                ItemsCount = 15,
                Number = 0
            };
        }
    }
}
