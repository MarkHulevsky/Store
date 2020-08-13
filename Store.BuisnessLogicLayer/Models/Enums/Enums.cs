namespace Store.BuisnessLogicLayer.Models.Enums
{
    public partial class Enums
    {
        public enum Currency
        {
            None = 0,
            USD = 1,
            EUR = 2,
            GBP = 3,
            CHF = 4,
            RUB = 5,
            PLN = 6
        }
        
        public enum OrderStatus
        {
            None = 0,
            Unpaid = 1,
            Paid = 2
        }
        public enum PrintingEditionType
        {
            None = 0,
            Book = 1,
            Newspapers = 2,
            Magazines = 3
        }
        public enum SortType
        {
            None= 0,
            Ascending = 1,
            Descending = 2
        }
    }
}
