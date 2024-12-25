namespace real_estate_api.Models
{
    public class Message
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Text {  get; set; }
        public string UserId {  get; set; }
        public string ChatId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Chat Chat { get; set; }
        public User User { get; set; }
    }
}
