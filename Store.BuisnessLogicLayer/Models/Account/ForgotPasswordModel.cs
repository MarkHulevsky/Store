using System.ComponentModel.DataAnnotations;

namespace Store.BuisnessLogicLayer.Models.Account
{
    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "Email required")]
        [EmailAddress(ErrorMessage = "Required email address format")]
        public string Email { get; set; }
    }
}
