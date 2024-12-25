namespace real_estate_api.Models
{
    public class Chat
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? LastMessage { get; set; }
        public ICollection<ChatUser> ChatUsers { get; set; } = new List<ChatUser>();

        // Quan hệ với các tin nhắn trong trò chuyện
        public ICollection<Message> Messages { get; set; } = new List<Message>();

        // Quan hệ với bảng ChatSeenBy để lưu thông tin người dùng đã xem
        public ICollection<ChatSeenBy> SeenByUsers { get; set; } = new List<ChatSeenBy>();
    }
}
