using System.ComponentModel.DataAnnotations;

namespace real_estate_api.DTOs
{
    public class UserDTO
    {
        public string Id { get; set; } 
        
        public string Email { get; set; } 
        public string Username { get; set; } 
        public string? Avatar { get; set; }
        public DateTime CreatedAt { get; set; } 
        public DateTime? UpdatedAt { get; set; } 
    }
}
