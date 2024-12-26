using real_estate_api.DTOs;
using real_estate_api.Models;

namespace real_estate_api.Interface.Repository
{
    public interface IMessageRepository
    {
        Task<Message> AddMessageAsync(MessageRequestDTO msRequest,string userId);
    }
}
