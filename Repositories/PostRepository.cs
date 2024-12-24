using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using real_estate_api.Data;
using real_estate_api.Enums;
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
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var post = await _context.Posts
                                .Include(p => p.SavedPosts) // Bao gồm SavedPosts liên quan
                                .FirstOrDefaultAsync(p => p.Id == id);
                if (post != null)
                {
                    _context.SavedPosts.RemoveRange(post.SavedPosts);
                    _context.Posts.Remove(post);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return true;
                }
                return false;
            }
           
            catch (Exception)
    {
                // Hoàn tác giao dịch nếu có lỗi
                await transaction.RollbackAsync();
                throw; // Ném lại ngoại lệ để xử lý ở nơi khác
            }
        }
        

        public async Task<IEnumerable<Post>> GetAllAsync()
        {
           return await _context.Posts.ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetAllWithUsersAndDetailsAsync(Query query)
        {
            var postsQuery = _context.Posts
             .Include(p => p.User) // Include related User data
             .Include(p => p.PostDetail) // Include related PostDetail data
             .OrderByDescending(p => p.CreatedAt) // Default ordering by CreatedAt
             .AsQueryable();

            if (!string.IsNullOrEmpty(query.City))
            {
                postsQuery = postsQuery.Where(p => p.City == query.City);
            }

            // Apply filtering by type if provided in the query
            if (!string.IsNullOrEmpty(query.Type))
            {
                if (Enum.TryParse<PostType>(query.Type, out var postType))
                {
                    postsQuery = postsQuery.Where(p => p.Type == postType);
                }
            }

            // Apply filtering by property if provided in the query
            if (!string.IsNullOrEmpty(query.Property))
            {
                // Assuming 'Property' is an enum (replace 'PropertyEnum' with the actual enum type)
                if (Enum.TryParse<PropertyType>(query.Property, out var property))
                {
                    postsQuery = postsQuery.Where(p => p.Property == property);
                }
            }

            // Apply filtering by bedroom count if provided in the query
            if (query.Bedroom.HasValue)
            {
                postsQuery = postsQuery.Where(p => p.Bedroom == query.Bedroom.Value);
            }

            // Apply price range if provided in the query
            if (query.MinPrice.HasValue)
            {
                postsQuery = postsQuery.Where(p => p.Price >= query.MinPrice.Value);
            }

            if (query.MaxPrice.HasValue)
            {
                postsQuery = postsQuery.Where(p => p.Price <= query.MaxPrice.Value);
            }
            return await postsQuery.ToListAsync();
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
        public async Task<IEnumerable<Post>> GetPostOfUserAsync(string userId)
        {
            return await _context.Posts
                .Include(p => p.PostDetail)
                .Where(P => P.UserId == userId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }


        public async Task UpdateAsync(Post post)
        {
            _context.Posts.Update(post);
        }
    }
}
