using System.ComponentModel.DataAnnotations;

namespace CookingAppMVC.Models
{
    public class ForgotPassword
    {
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"(?=^.{8,}$)((?=.\d)|(?=.\W+))(?![.\n])(?=.[A-Z])(?=.[a-z]).*$", ErrorMessage = "Password should contain 8 characters, one uppercase, one lowercase, and one special character at least.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}