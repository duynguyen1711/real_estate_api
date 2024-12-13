using AutoMapper;
using real_estate_api.DTOs;
using real_estate_api.Interface.Service;
using real_estate_api.Models;
using real_estate_api.UnitofWork;

namespace real_estate_api.Services
{
    public class PostDetailService : IPostDetailService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public PostDetailService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public  async Task UpdatePostDetail(Post post, PostDetailUpdateDTO postDetailDTO)
        {
            if (post == null)
            {
                throw new ArgumentNullException(nameof(post), "Post cannot be null");
            }

            if (postDetailDTO == null)
            {
                return; // Không có gì để cập nhật
            }

            // Map DTO sang entity
            var updatedPostDetail = _mapper.Map<PostDetail>(postDetailDTO);

            if (post.PostDetail != null)
            {
                // Cập nhật từng trường của PostDetail
                post.PostDetail.Description = updatedPostDetail.Description ?? post.PostDetail.Description;
                post.PostDetail.Utilities = updatedPostDetail.Utilities ?? post.PostDetail.Utilities;
                post.PostDetail.PetPolicy = updatedPostDetail.PetPolicy ?? post.PostDetail.PetPolicy;
                post.PostDetail.IncomeRequirement = updatedPostDetail.IncomeRequirement ?? post.PostDetail.IncomeRequirement;
                post.PostDetail.Size = updatedPostDetail.Size ?? post.PostDetail.Size;
                post.PostDetail.NearbySchools = updatedPostDetail.NearbySchools ?? post.PostDetail.NearbySchools;
                post.PostDetail.NearbyBusStops = updatedPostDetail.NearbyBusStops ?? post.PostDetail.NearbyBusStops;
                post.PostDetail.NearbyRestaurants = updatedPostDetail.NearbyRestaurants ?? post.PostDetail.NearbyRestaurants;
            }
            else
            {
                // Nếu PostDetail chưa tồn tại, tạo mới
                post.PostDetail = updatedPostDetail;
            }
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
