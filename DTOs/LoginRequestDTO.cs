using System.ComponentModel.DataAnnotations;

namespace real_estate_api.DTOs
{
    public class LoginRequestDTO
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Passoword is required.")]
        public string Password { get; set; }
    }
}
