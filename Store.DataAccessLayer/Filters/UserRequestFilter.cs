using System;
using System.Collections.Generic;
using System.Text;
using static Store.DataAccessLayer.Entities.Enums.Enums;

namespace Store.DataAccessLayer.Filters
{
    public class UserRequestFilter
    {
        public string PropName { get; set; }
        public List<UserStatus> Statuses { get; set; }
        public SortType SortType { get; set; }
        public string SearchFilter { get; set; }
        public Paging Paging { get; set; }
        public UserRequestFilter()
        {
            Paging = new Paging
            {
                ItemsCount = 15,
                Number = 0
            };
        }
    }
}
