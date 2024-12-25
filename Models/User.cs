namespace real_estate_api.Models
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Avatar { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } 

        // Navigation properties
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<SavedPost> SavedPosts { get; set; } = new List<SavedPost>();
        public ICollection<ChatUser> ChatUsers { get; set; } = new List<ChatUser>();

        // Quan hệ với bảng ChatSeenBy
        public ICollection<ChatSeenBy> ChatSeenByUsers { get; set; } = new List<ChatSeenBy>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
