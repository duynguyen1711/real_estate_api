using System.ComponentModel.DataAnnotations;

namespace real_estate_api.DTOs
{
    public class UserDTO
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string? Avatar { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } 
    }
}
