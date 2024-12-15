using real_estate_api.Enums;

namespace real_estate_api.Models
{
    public class Post
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; } 
        public int Price { get; set; }
        public List<string>? Images { get; set; } 
        public string Address { get; set; } 
        public string City { get; set; } 
        public int Bedroom { get; set; }
        public int Bathroom { get; set; }
        public string Latitude { get; set; } 
        public string Longitude { get; set; } 
        public PostType Type { get; set; }
        public PropertyType Property { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string UserId { get; set; }
        public User User { get; set; }
        public PostDetail? PostDetail { get; set; } // Navigation Property
        public List<SavedPost> SavedPosts { get; set; } = new List<SavedPost>();
    }
}
