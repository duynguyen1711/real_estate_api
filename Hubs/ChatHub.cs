using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using real_estate_api.DTOs;
using real_estate_api.Interface.Service;
using real_estate_api.Models;
using real_estate_api.Services;
using System;
using System.Security.Claims;


namespace real_estate_api.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IMessageService _messageService;
        private readonly IChatService _chatService;
        public ChatHub(IMessageService messageService, IChatService chatService)
        {
            _messageService = messageService;
            _chatService = chatService;
        }
        public async Task SendMessageAsync(string chatId, string message)
        {
            // Lấy ID người gửi từ claims
            var currentUserId = Context.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(currentUserId))
            {
                throw new HubException("Unauthorized user");
            }

            // Tạo DTO cho tin nhắn
            var messageRequestDTO = new MessageRequestDTO
            {
                Text = message,
                ChatId = chatId // Đây là ID của cuộc trò chuyện
            };

            // Lưu tin nhắn vào database
            var responseDTO = await _messageService.AddMessageAsync(messageRequestDTO, currentUserId);

            // Lấy thông tin chat từ database
            var chat = await _chatService.GetChatAsync(chatId, currentUserId);

            if (chat == null)
            {
                throw new HubException("Chat not found or you are not authorized to access it.");
            }

            // Xác định người nhận từ UserIDs, là người duy nhất không phải là người gửi
            var receiverId = chat.UserIDs.FirstOrDefault(id => id != currentUserId);

            if (string.IsNullOrEmpty(receiverId))
            {
                throw new HubException("Receiver not found.");
            }
            var covertTime = responseDTO.CreatedAt.AddHours(-7);
            // Gửi tin nhắn cho người nhận
            await Clients.User(receiverId).SendAsync("ReceiveMessage", responseDTO.Text, covertTime);
        }


    }
}
