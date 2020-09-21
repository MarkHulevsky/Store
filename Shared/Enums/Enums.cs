﻿namespace Shared.Enums
{
    public partial class Enums
    {
        public enum OrderStatus
        {
            None = 0,
            Unpaid = 1,
            Paid = 2
        }
        public enum CurrencyType
        {
            None = 0,
            USD = 1,
            EUR = 2,
            GBP = 3,
            CHF = 4,
            RUB = 5,
            PLN = 6
        }
        public enum SortType
        {
            None = 0,
            Asc = 1,
            Desc = 2
        }
        public enum PrintingEditionType
        {
            None = 0,
            Book = 1,
            Newspaper = 2,
            Magazine = 3
        }
    }
}
