using System.ComponentModel.DataAnnotations;

namespace CookingAppMVC.Models
{
    public class Admin
    {
        public int AdminId { get; set; }

        [Required(ErrorMessage = "Please enter an admin name.")]
        public string Adminname { get; set; }

        [Required(ErrorMessage = "Please enter an email address.")]
        [RegularExpression(@"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$", ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*[@#$%^&+=!]).{8,}$", ErrorMessage = "Password should contain 8 characters, one uppercase, one lowercase, and one special character at least.")]
        public string Password { get; set; }
    }
}
