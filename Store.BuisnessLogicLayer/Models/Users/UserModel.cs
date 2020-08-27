using Store.BuisnessLogic.Models.Base;
using Store.BuisnessLogic.Models.Orders;
using System.Collections.Generic;

namespace Store.BuisnessLogic.Models.Users
{
    public class UserModel : BaseModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Password { get; set; }
        public List<string> Roles { get; set; }
        public bool IsActive { get; set; }
        public List<OrderModel> Orders { get; set; }
        public UserModel()
        {
            Roles = new List<string>();
            Orders = new List<OrderModel>();
            IsActive = true;
        }
    }
}
