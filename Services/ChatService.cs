using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
            try
            {
                // Tạo cuộc trò chuyện mới thông qua repository
                var chat = await _unitOfWork.ChatRepository.CreateChatAsync(userIds);
                await _unitOfWork.SaveChangesAsync();
                var chatDTO = new ChatDTOResponse
                {
                    Id = chat.Id,
                    CreatedAt = chat.CreatedAt,
                    LastMessage = chat.LastMessage,
                    Messages = chat.Messages.Select(msg => new MessageResponeDTO
                    {
                        Id = msg.Id,
                        Text = msg.Text,
                        UserId = msg.UserId,
                        CreatedAt = msg.CreatedAt,
                        ChatId = msg.ChatId
                    }).ToList(),  // Nếu muốn, có thể ánh xạ các thông tin liên quan đến messages
                    UserIDs = chat.ChatUsers.Select(cu => cu.UserId).ToList(),
                    SeenBy = chat.SeenByUsers
                    .Where(s => s.IsSeen == true)
                    .Select(s => s.UserId)
                    .ToList()
                };


                return chatDTO;
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("A chat between these users already exists.", ex);
            }
        }

        public async Task<ChatDTOResponse> GetChatAsync(string chatId, string userId)
        {
            var chat = await _unitOfWork.ChatRepository.GetChatByUserAsync(chatId, userId);
            // Gọi repository để lấy chat với chatId
            if (chat == null)
            {
                throw new ArgumentException("Chat not found or you are not authorized to access it.");
            }
            var chatDTO = new ChatDTOResponse
            {
                Id = chat.Id,
                CreatedAt = chat.CreatedAt,
                LastMessage = chat.LastMessage,
                Messages = chat.Messages.Select(msg => new MessageResponeDTO
                {
                    Id = msg.Id,
                    Text = msg.Text,
                    UserId = msg.UserId,
                    CreatedAt = msg.CreatedAt,
                    ChatId = msg.ChatId
                }).ToList(),
                UserIDs = chat.ChatUsers.Select(cu => cu.UserId).ToList(),
                SeenBy =  chat.SeenByUsers
                    .Where(s => s.IsSeen == true)
                    .Select(s => s.UserId)
                    .ToList()
            };


            return chatDTO;
        }

        public async Task<List<ChatDTOResponse>> GetChatsAsync(string userId)
        {
            var chatList = await _unitOfWork.ChatRepository.GetChatsByUserAsync(userId);

            // Ánh xạ từ model Chat sang ChatDTOResponse
            var chatDTOList = chatList.Select(chat => new ChatDTOResponse
            {
                Id = chat.Id,
                UserIDs = chat.ChatUsers.Select(cu => cu.UserId).ToList(),
                CreatedAt = chat.CreatedAt,
                SeenBy = chat.SeenByUsers
                    .Where(s => s.IsSeen == true)
                    .Select(s => s.UserId)
                    .ToList(), // Trích xuất danh sách UserId đã xem
                LastMessage = chat.LastMessage,
                Messages = chat.Messages.Select(msg => new MessageResponeDTO
                {
                    Id = msg.Id,
                    Text = msg.Text,
                    UserId = msg.UserId,
                    CreatedAt = msg.CreatedAt,
                    ChatId = msg.ChatId
                }).ToList(),
                Receiver = chat.ChatUsers
                    .Where(cu => cu.UserId != userId) 
                    .Select(cu => new ReceiverDTO
                    {
                        Avatar = cu.User.Avatar,
                        Username = cu.User.Username,
                    }).ToList(),

            }).ToList();

            return chatDTOList;
        }

        public async Task MarkChatAsReadAsync(string chatId, string userId)
        {           
            // Cập nhật trạng thái `IsSeen` trong bảng ChatSeenBy
            await _unitOfWork.ChatRepository.MarkChatAsReadAsync(chatId, userId);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}

