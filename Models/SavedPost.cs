namespace real_estate_api.Models
{
    public class SavedPost
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; }
        public User User { get; set; }
        public string PostId { get; set; }
        public Post Post { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
