using real_estate_api.Data;
using real_estate_api.DTOs;
using real_estate_api.Interface.Repository;
using real_estate_api.Models;

namespace real_estate_api.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _context;

        public MessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async  Task<Message> AddMessageAsync(MessageRequestDTO msRequest, string userId)
        {
            var chat = await _context.Chats.FindAsync(msRequest.ChatId);
            if (chat == null)
            {
                throw new ArgumentException("Chat not found.");
            }
            // Cập nhật LastMessage
            chat.LastMessage = msRequest.Text;
            var message = new Message
            {
                Text = msRequest.Text,
                UserId = userId,
                ChatId = msRequest.ChatId, 
            };
            await _context.Messages.AddAsync(message);
            return message;
        }
    }
}
