using real_estate_api.DTOs;
using real_estate_api.Models;

namespace real_estate_api.Interface.Service
{
    public interface IChatService
    {
        Task<ChatDTOResponse> CreateChatAsync(List<string> userIds);
        Task<ChatDTOResponse> GetChatAsync(string chatId);
        Task<List<ChatDTOResponse>> GetChatsAsync();
    }
}
