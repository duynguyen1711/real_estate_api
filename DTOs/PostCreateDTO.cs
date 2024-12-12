using real_estate_api.Enums;
using real_estate_api.Models;

namespace real_estate_api.DTOs
{
    public class PostCreateDTO
    {
        public string Title { get; set; } 
        public int Price { get; set; }
        public List<string>? Images { get; set; } = new List<string>();
        public string Address { get; set; } 
        public string City { get; set; } 
        public int Bedroom { get; set; }
        public int Bathroom { get; set; }
        public string Latitude { get; set; } 
        public string Longitude { get; set; } 
        public PostType Type { get; set; }
        public PropertyType Property { get; set; }
        public PostDetailCreateDTO? PostDetail { get; set; }
    }
}
