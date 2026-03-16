using System.ComponentModel.DataAnnotations;

namespace HotelMind.Models
{
    public class LoginDBModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email.")]
        [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "The password must be at least 6 characters.")]
        public string Password { get; set; }
    }
}
