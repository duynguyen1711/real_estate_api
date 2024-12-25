namespace real_estate_api.Models
{
    public class ChatSeenBy 
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ChatId { get; set; } // Khóa ngoại tới bảng Chat
        public Chat Chat { get; set; }

        public string UserId { get; set; } // Khóa ngoại tới bảng User
        public User User { get; set; }

        public DateTime SeenAt { get; set; } = DateTime.UtcNow;
    }
}
