using Store.BuisnessLogicLayer.Models.Base;
using Store.BuisnessLogicLayer.Models.Orders;
using System.Collections.Generic;

namespace Store.BuisnessLogicLayer.Models.Users
{
    public class UserModel : BaseModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Password { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
        public bool IsActive { get; set; } = true;
        public List<OrderModel> Orders { get; set; } = new List<OrderModel>();
    }
}
