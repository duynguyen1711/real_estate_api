namespace real_estate_api.Models
{
    public class PostDetail
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Description { get; set; } = string.Empty;
        public string? Utilities { get; set; }
        public string? PetPolicy { get; set; }
        public string? IncomeRequirement { get; set; }
        public int? Size { get; set; }
        public int? NearbySchools { get; set; }
        public int? NearbyBusStops { get; set; }
        public int? NearbyRestaurants { get; set; }
        public string PostId { get; set; } // Foreign Key
        public Post Post { get; set; }
    }
}
