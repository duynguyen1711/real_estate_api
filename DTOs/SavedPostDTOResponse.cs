namespace real_estate_api.DTOs
{
    public class SavedPostDTOResponse
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public List<string> Images { get; set; }
        public int Price { get; set; }
        public string Address { get; set; }
        public int Bedroom { get; set; }
        public int Bathroom { get; set; }
        public bool isSaved { get; set; }
    }

    
}
