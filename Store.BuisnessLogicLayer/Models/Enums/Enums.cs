namespace Store.BuisnessLogicLayer.Models.Enums
{
    public partial class Enums
    {
        public enum Currency
        {
            USD = 0,
            EUR = 1,
            GBR = 2,
            CHF = 3,
            JPU = 4,
            UAH = 5
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
            Book = 0,
            Newspapers = 1,
            Magazines = 2
        }
        public enum SortType
        {
            Ascending = 0,
            Descending = 1
        }
    }
}
