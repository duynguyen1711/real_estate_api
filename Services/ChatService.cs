using AutoMapper;
using real_estate_api.DTOs;
using real_estate_api.Interface.Service;
using real_estate_api.Models;
using real_estate_api.UnitofWork;
using System;

namespace real_estate_api.Services
{
    public class ChatService : IChatService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        
        public ChatService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async  Task<ChatDTOResponse> CreateChatAsync(List<string> userIds)
        {
            if (userIds == null || userIds.Count < 2)
            {
                throw new ArgumentException("At least two users are required to create a chat.");
            }

            // Tạo cuộc trò chuyện mới thông qua repository
            var chat = await _unitOfWork.ChatRepository.CreateChatAsync(userIds);
            await _unitOfWork.SaveChangesAsync();
            var chatDTO = new ChatDTOResponse
            {
                Id = chat.Id,
                CreatedAt = chat.CreatedAt,
                LastMessage = chat.LastMessage,
                Messages = chat.Messages,  // Nếu muốn, có thể ánh xạ các thông tin liên quan đến messages
                UserIDs = chat.ChatUsers.Select(cu => cu.UserId).ToList(),
                SeenBy = chat.SeenByUsers.Select(sb => sb.UserId).ToList()
            };


            return chatDTO;
        }

        public async Task<ChatDTOResponse> GetChatAsync(string chatId)
        {
            var chat = await _unitOfWork.ChatRepository.GetChatAsync(chatId);
            // Gọi repository để lấy chat với chatId
            var chatDTO = new ChatDTOResponse
            {
                Id = chat.Id,
                CreatedAt = chat.CreatedAt,
                LastMessage = chat.LastMessage,
                Messages = chat.Messages,  // Nếu muốn, có thể ánh xạ các thông tin liên quan đến messages
                UserIDs = chat.ChatUsers.Select(cu => cu.UserId).ToList(),
                SeenBy = chat.SeenByUsers.Select(sb => sb.UserId).ToList()
            };


            return chatDTO;
        }

        public async Task<List<ChatDTOResponse>> GetChatsAsync()
        {
            var chatList = await _unitOfWork.ChatRepository.GetChatsAsync();

            // Ánh xạ từ model Chat sang ChatDTOResponse
            var chatDTOList = chatList.Select(chat => new ChatDTOResponse
            {
                Id = chat.Id,
                UserIDs = chat.ChatUsers.Select(cu => cu.UserId).ToList(), // Trích xuất chỉ UserId
                CreatedAt = chat.CreatedAt,
                SeenBy = chat.SeenByUsers.Select(s => s.UserId).ToList(), // Trích xuất danh sách UserId đã xem
                LastMessage = chat.LastMessage,
                Messages = chat.Messages // Nếu cần có thông tin các tin nhắn
            }).ToList();

            return chatDTOList;
        }
    }
}

