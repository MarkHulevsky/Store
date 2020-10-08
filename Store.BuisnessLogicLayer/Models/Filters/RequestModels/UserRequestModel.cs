namespace Store.BuisnessLogic.Models.Filters
{
    public class UserRequestModel : BaseRequestModel
    {
        public string SortPropertyName { get; set; }
        public string SearchString { get; set; }
    }
}
