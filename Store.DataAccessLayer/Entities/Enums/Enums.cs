namespace Store.DataAccessLayer.Entities.Enums
{
    public partial class Enums
    {
        public enum Currency
        {
            USD = 1,
            EUR,
            GBR,
            CHF,
            JPU,
            UAH
        }
        public enum UserStatus
        {
            Active = 0,
            Blocked
        }
        public enum OrderStatus
        {
            Unpaid = 0,
            Paid
        }
        public enum PrintingEditionType
        {
            Book = 1,
            Newspapers,
            Magazines
        }
        public enum SortType
        {
            Ascending = 1,
            Descending
        }
        
    }
}
