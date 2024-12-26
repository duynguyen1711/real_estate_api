using real_estate_api.DTOs;

namespace real_estate_api.Interface.Service
{
    public interface IMessageService
    {
        Task<MessageResponeDTO> AddMessageAsync (MessageRequestDTO messageRequestDTO,string userId);
    }
}
