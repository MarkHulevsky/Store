using System.ComponentModel.DataAnnotations;

namespace Store.Presentation.Models.AccountModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Email required")]
        [EmailAddress(ErrorMessage = "Required email address format")]
        public string Email { get; set; }
    }
}
