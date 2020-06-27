using Store.BuisnessLogicLayer.Models.Base;
using static Store.BuisnessLogicLayer.Models.Enums.Enums;

namespace Store.BuisnessLogicLayer.Models.Users
{
    public class UserModel : BaseModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Password { get; set; }
        public UserStatus Status { get; set; }
    }
}
