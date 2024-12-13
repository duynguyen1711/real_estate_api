using real_estate_api.Models;

namespace real_estate_api.DTOs
{
    public class PostDetailCreateDTO
    {
        public string? Description { get; set; } 
        public string? Utilities { get; set; }
        public string? PetPolicy { get; set; }
        public string? IncomeRequirement { get; set; }
        public int? Size { get; set; }
        public int? NearbySchools { get; set; }
        public int? NearbyBusStops { get; set; }
        public int? NearbyRestaurants { get; set; }
    }
}
