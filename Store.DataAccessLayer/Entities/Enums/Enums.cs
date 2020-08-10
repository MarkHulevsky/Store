namespace Store.DataAccessLayer.Entities.Enums
{
    public partial class Enums
    {
        public enum Currency
        {
            USD = 0,
            EUR = 1,
            GBP = 2,
            CHF = 3,
            RUB = 4,
            PLN = 5
        }
        public enum UserStatus
        {
            Active = 0,
            Blocked = 1
        }
        public enum OrderStatus
        {
            Unpaid = 0,
            Paid = 1
        }
        public enum PrintingEditionType
        {
            Book = 1,
            Newspapers = 2,
            Magazines = 3
        }
        public enum SortType
        {
            Ascending = 0,
            Descending = 1
        }
        
    }
}
