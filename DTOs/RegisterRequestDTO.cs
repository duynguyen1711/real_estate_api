using System.ComponentModel.DataAnnotations;

namespace real_estate_api.DTOs
{
    public class RegisterRequestDTO
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Passoword is required.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
    }
}
