namespace real_estate_api.Models
{
    public class Query
    {
        public string? City { get; set; }
        public string? Type { get; set; }
        public string? Property { get; set; }
        public int? Bedroom { get; set; }
        public int? MinPrice { get; set; } 
        public int? MaxPrice { get; set; } 
    }
}
