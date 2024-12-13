using Microsoft.EntityFrameworkCore;
using real_estate_api.Data;
using real_estate_api.Interface.Repository;
using real_estate_api.Models;

namespace real_estate_api.Repositories
{
    public class PostRepository:IPostRepository
    {
        private readonly ApplicationDbContext _context;

        public PostRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Post post)
        {
           await _context.Posts.AddAsync(post);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        

        public async Task<IEnumerable<Post>> GetAllAsync()
        {
           return await _context.Posts.ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetAllWithUsersAndDetailsAsync()
        {
            return await _context.Posts
                .Include(p => p.User) // Thêm thông tin User
                .Include(p => p.PostDetail) // Thêm chi tiết bài đăng
                .ToListAsync();

        }

        public async Task<Post> GetPost(string id)
        {
            return await _context.Posts.FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<Post> GetPostWithDetailsAsync(string postId)
        {
            return await _context.Posts.Include(p => p.PostDetail)
                                        .FirstOrDefaultAsync(p => p.Id == postId);
        }
        public async Task<Post> GetPostWithUserAndDetailAsync(string postId)
        {
            return await _context.Posts
                .Where(p => p.Id == postId) 
                .Include(p => p.User) 
                .Include(p => p.PostDetail) 
                .FirstOrDefaultAsync(); 
        }


        public async Task UpdateAsync(Post post)
        {
            _context.Posts.Update(post);
        }
    }
}
