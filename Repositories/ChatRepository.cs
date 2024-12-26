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
                .Where(g => g.Count() == userIds.Count && g.All(cu => userIds.Contains(cu.UserId))) // Kiểm tra nếu tất cả người dùng trong nhóm có trong danh sách userIds
                .Select(g => g.Key)
                .FirstOrDefaultAsync();

            if (existingChat != null)
            {
                throw new InvalidOperationException("Chat between these users already exists.");
            }

            // Nếu chưa có chat, tạo một chat mới
            var chat = new Chat
            {
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
                var chatSeenBy = new ChatSeenBy
                {
                    ChatId = chat.Id,
                    UserId = userId
                };
                _context.ChatSeenBy.Add(chatSeenBy);
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
        public async Task<Chat> GetChatByUserAsync(string chatId,string userId )
        {
            var chat = await _context.Chats
               .Where(c => c.Id == chatId && c.ChatUsers.Any(cu => cu.UserId == userId))
               .Include(c => c.ChatUsers) // Bao gồm thông tin người dùng trong cuộc trò chuyện
                   .ThenInclude(cu => cu.User)
               .Include(c => c.Messages.OrderByDescending(m => m.CreatedAt))
               .Include(c => c.SeenByUsers)
                   .ThenInclude(sbu => sbu.User)
               .FirstOrDefaultAsync();

            // Kiểm tra nếu không tìm thấy chat (chat == null)
            if (chat == null)
            {
                return null;  
            }

            return chat;
        }

        public async Task<List<Chat>> GetChatsAsync()
        {
            return await _context.Chats
            .Include(c => c.ChatUsers)
                .ThenInclude(cu => cu.User)
            .Include(c => c.Messages)
            .ToListAsync();
        }
        public async Task<List<Chat>> GetChatsByUserAsync(string userId)
        {
            return await _context.Chats
                .Where(c => c.ChatUsers.Any(cu => cu.UserId == userId)) // Lọc các cuộc trò chuyện mà người dùng tham gia
                .Include(c => c.ChatUsers)
                    .ThenInclude(cu => cu.User) // Bao gồm thông tin người dùng trong mỗi cuộc trò chuyện
                .Include(c => c.Messages.OrderByDescending(m => m.CreatedAt))
                .Include(c => c.SeenByUsers)
                   .ThenInclude(sbu => sbu.User)
                .OrderByDescending(c => c.Messages.Max(m => m.CreatedAt))
                .ToListAsync();
        }

        public async Task MarkChatAsReadAsync(string chatId, string userId)
        {
            var chatSeen = await _context.ChatSeenBy
                .FirstOrDefaultAsync(csb => csb.ChatId == chatId && csb.UserId == userId);
            if (chatSeen != null)
            {
                // Cập nhật trạng thái IsSeen
                chatSeen.IsSeen = true;
                chatSeen.SeenAt = DateTime.UtcNow;

                // Lưu thay đổi
                
            }
            else
            {
                // Nếu chưa có, tạo bản ghi mới
                _context.ChatSeenBy.Add(new ChatSeenBy
                {    
                    ChatId = chatId,
                    UserId = userId,
                    IsSeen = true,
                });

              
            }
        }
    }
}
