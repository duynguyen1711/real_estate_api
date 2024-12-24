using Microsoft.EntityFrameworkCore;
using real_estate_api.Data;
using real_estate_api.Interface.Repository;
using real_estate_api.Models;

namespace real_estate_api.Repositories
{
    public class SavedPostRepository : ISavedPostRepository
    {
        private readonly ApplicationDbContext _context;

        public SavedPostRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddSavedPost(SavedPost savedPost)
        {
            await _context.SavedPosts.AddAsync(savedPost);
        }

        public async Task DeleteSavedPost(SavedPost savedPost)
        {
            _context.SavedPosts.Remove(savedPost);
        }

        public async Task<List<SavedPost>> GetListSavedPostOfUser(string userID)
        {
            return await _context.SavedPosts
            .Where(sp => sp.UserId == userID)  
            .Include(sp => sp.Post)  
            .ToListAsync();
        }

        public async Task<SavedPost> GetSavedPost(string userID, string PostId)
        {
            return await _context.SavedPosts
                .FirstOrDefaultAsync(sp => sp.UserId == userID && sp.PostId == PostId);
        }
    }
}
