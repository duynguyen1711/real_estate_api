using Microsoft.EntityFrameworkCore;
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
            // Kiểm tra nếu PostDetail đã tồn tại trong cơ sở dữ liệu
            var existingPostDetail = await _context.PostDetails
                .FirstOrDefaultAsync(p => p.PostId == postDetail.PostId);

            if (existingPostDetail != null)
            {
                // Nếu tồn tại, cập nhật thông tin thay vì thêm mới
                existingPostDetail.Description = postDetail.Description;
                existingPostDetail.Utilities = postDetail.Utilities;
                existingPostDetail.PetPolicy = postDetail.PetPolicy;
                existingPostDetail.IncomeRequirement = postDetail.IncomeRequirement;
                existingPostDetail.Size = postDetail.Size;
                existingPostDetail.NearbySchools = postDetail.NearbySchools;
                existingPostDetail.NearbyBusStops = postDetail.NearbyBusStops;
                existingPostDetail.NearbyRestaurants = postDetail.NearbyRestaurants;

                _context.PostDetails.Update(existingPostDetail);  // Cập nhật bản ghi
            }
            else
            {
                // Nếu không tồn tại, thêm mới
                await _context.PostDetails.AddAsync(postDetail);
            }

            return postDetail;
        }

    }
}
