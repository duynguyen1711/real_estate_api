using Microsoft.Extensions.Hosting;
using real_estate_api.Data;
using real_estate_api.Interface.Repository;
using real_estate_api.Models;

namespace real_estate_api.Repositories
{
    public class PostDetailRepository:IPostDetailRepository
    {
        private readonly ApplicationDbContext _context;

        public PostDetailRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PostDetail> UpdateAsync(PostDetail postDetail)
        {
            _context.PostDetails.Update(postDetail);
            return postDetail;
        }
    }
}
