using Microsoft.EntityFrameworkCore;
using real_estate_api.Data;
using real_estate_api.Interface.Repository;
using real_estate_api.Models;

namespace real_estate_api.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly ApplicationDbContext _context;

        public ChatRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Chat> CreateChatAsync(List<string> userIds)
        {
            if (userIds == null || userIds.Count < 2)
            {
                throw new ArgumentException("At least two users are required to create a chat.");
            }

            // Sắp xếp danh sách userIds để tránh việc so sánh ngược (User A - User B và User B - User A)
            userIds.Sort();

            // Kiểm tra xem chat giữa 2 người dùng đã tồn tại chưa
            var existingChat = await _context.ChatUsers
                .Where(cu => userIds.Contains(cu.UserId))
                .GroupBy(cu => cu.ChatId)
                .Where(g => g.All(cu => userIds.Contains(cu.UserId)))  // Kiểm tra nếu tất cả người dùng trong nhóm có trong danh sách userIds
                .Select(g => g.Key)
                .FirstOrDefaultAsync();

            if (existingChat != null)
            {
                // Nếu chat đã tồn tại, trả về chat hiện có
                return await _context.Chats
                    .Include(c => c.ChatUsers)
                    .ThenInclude(cu => cu.User)
                    .FirstOrDefaultAsync(c => c.Id == existingChat);
            }

            // Nếu chưa có chat, tạo một chat mới
            var chat = new Chat
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                LastMessage = null // bạn có thể cập nhật tin nhắn sau khi có tin nhắn mới
            };

            // Lưu chat vào cơ sở dữ liệu
            _context.Chats.Add(chat);

            // Tạo các bản ghi ChatUser để thêm người dùng vào cuộc trò chuyện
            foreach (var userId in userIds)
            {
                var chatUser = new ChatUser
                {
                    ChatId = chat.Id,
                    UserId = userId
                };
                _context.ChatUsers.Add(chatUser);
            }

            return chat;
        }

        public async Task<Chat> GetChatAsync(string chatId)
        {
            return await _context.Chats
                .Where(c => c.Id == chatId)
                .Include(c => c.ChatUsers)
                    .ThenInclude(cu => cu.User)
                .Include(c => c.Messages)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Chat>> GetChatsAsync()
        {
            return await _context.Chats
            .Include(c => c.ChatUsers)
                .ThenInclude(cu => cu.User)
            .Include(c => c.Messages)
            .ToListAsync();
        }
    }
}
