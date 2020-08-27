using System.ComponentModel.DataAnnotations;

namespace Store.BuisnessLogic.Models.Account
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Email required")]
        [EmailAddress(ErrorMessage = "Required email address format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "First name required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Password required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password required")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords must match")]
        public string ConfirmPassword { get; set; }
    }
}
