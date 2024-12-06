using System.ComponentModel.DataAnnotations;

namespace real_estate_api.DTOs
{
    public class UpdateUserDTO
    {
        [EmailAddress]
        public string? Email { get; set; }
        public string? Username { get; set; } 
        public string? Avatar { get; set; }
        public string? Password { get; set; } 
    }
}
