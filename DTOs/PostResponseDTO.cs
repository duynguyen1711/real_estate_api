using real_estate_api.Enums;
using real_estate_api.Models;

namespace real_estate_api.DTOs
{
    public class PostResponseDTO
    {
        public string Id { get; set; } 
        public string Title { get; set; }
        public int Price { get; set; }
        public List<string> Images { get; set; }
        public string Address { get; set; } 
        public string City { get; set; } 
        public int Bedroom { get; set; }
        public int Bathroom { get; set; }
        public string Latitude { get; set; } 
        public string Longitude { get; set; } 
        public PostType Type { get; set; }
        public PropertyType Property { get; set; }
        public DateTime CreatedAt { get; set; } 
        public string UserId { get; set; }
        public UserDTOForPost User { get; set; }
        public PostDetailCreateDTO PostDetail { get; set; }
    }
}
