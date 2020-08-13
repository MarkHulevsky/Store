using System.ComponentModel.DataAnnotations;

namespace Store.BuisnessLogicLayer.Models.Account
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email required")]
        [EmailAddress(ErrorMessage = "Required email address format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
