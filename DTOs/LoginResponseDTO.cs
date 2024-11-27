namespace real_estate_api.DTOs
{
    public class LoginResponseDTO
    {
        public string Message { get; set; }
        public string Token { get; set; } 
        public string Username { get; set; } 
        public string UserId { get; set; }
        public string? Avatar { get; set; }
        public string Email { get; set; }
    }
}
