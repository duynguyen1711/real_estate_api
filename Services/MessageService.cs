using AutoMapper;
using real_estate_api.DTOs;
using real_estate_api.Interface.Service;
using real_estate_api.Models;
using real_estate_api.UnitofWork;

namespace real_estate_api.Services
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public MessageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<MessageResponeDTO> AddMessageAsync(MessageRequestDTO messageRequestDTO,string userId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            var createdMessage = await _unitOfWork.MessageRepository.AddMessageAsync(messageRequestDTO,user.Id);
            var chat = await _unitOfWork.ChatRepository.GetChatAsync(messageRequestDTO.ChatId);
            if (chat == null)
            {
                throw new ArgumentException("Chat not found.");
            }
            chat.LastMessage = messageRequestDTO.Text;
            await _unitOfWork.SaveChangesAsync();
            var responseDTO = new MessageResponeDTO
            {
                Id = createdMessage.Id,
                Text = createdMessage.Text,
                UserId = createdMessage.UserId,
                ChatId = createdMessage.ChatId,
                CreatedAt = createdMessage.CreatedAt
            };

            return responseDTO;
        }
    }
}
