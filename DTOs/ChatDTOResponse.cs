using real_estate_api.Models;

namespace real_estate_api.DTOs
{
    public class ChatDTOResponse
    {
        public string Id { get; set; }
        public List<string> UserIDs { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string> SeenBy { get; set; }
        public string? LastMessage { get; set; }
        public ICollection<MessageResponeDTO> Messages { get; set; } = new List<MessageResponeDTO>();
        public List<ReceiverDTO> Receiver { get; set; } = new List<ReceiverDTO>();
    }
}