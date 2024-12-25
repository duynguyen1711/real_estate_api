using real_estate_api.Models;

namespace real_estate_api.Interface.Repository
{
    public interface IChatRepository
    {
        Task<Chat> CreateChatAsync(List<string> userIds);
        Task <List<Chat>> GetChatsAsync();
        Task<Chat> GetChatAsync(string chatId);
    }
}
