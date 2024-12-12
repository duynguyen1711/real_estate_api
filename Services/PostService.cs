using AutoMapper;
using Microsoft.Extensions.Hosting;
using real_estate_api.DTOs;
using real_estate_api.Helpers;
using real_estate_api.Interface.Service;
using real_estate_api.Models;
using real_estate_api.UnitofWork;

namespace real_estate_api.Services
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PostService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddPostAsync(PostCreateDTO postDTO, string id)
        {

            var post = _mapper.Map<Post>(postDTO);
            post.UserId = id;
            post.CreatedAt = DateTime.Now;
            await _unitOfWork.PostRepository.AddAsync(post);
            await _unitOfWork.SaveChangesAsync();



        }

        public async Task<IEnumerable<PostResponseDTO>> GetAllPostAsync()
        {
            var posts = await _unitOfWork.PostRepository.GetAllAsync();
            var postDTOs = new List<PostResponseDTO>();


            foreach (var post in posts)
            {
                var postDTO = _mapper.Map<PostResponseDTO>(post);

                // Gọi hàm để lấy thông tin người dùng từ UserRepository (dùng 1 ID mỗi lần)
                var user = await _unitOfWork.UserRepository.GetByIdAsync(post.UserId);


                // Gán thông tin người dùng vào DTO
                postDTO.User = new UserDTOForPost
                {
                    Username = user.Username,
                    Avatar = user.Avatar
                };


                postDTOs.Add(postDTO);
            }

            return postDTOs;
        }
        public async Task<IEnumerable<PostResponseDTO>> GetAllPostWithDetailAsync()
        {
            var posts = await _unitOfWork.PostRepository.GetAllWithUsersAndDetailsAsync();
            var postDTOs = posts.Select(post =>
            {
                // Kiểm tra null trước khi truy cập thuộc tính
                var userDTO = post.User != null ? new UserDTOForPost
                {
                    Username = post.User.Username,
                    Avatar = post.User.Avatar
                } : null;

                var postDetailDTO = post.PostDetail != null ? new PostDetailCreateDTO
                {
                    Description = post.PostDetail.Description,
                    Utilities = post.PostDetail.Utilities,
                    PetPolicy = post.PostDetail.PetPolicy,
                    IncomeRequirement = post.PostDetail.IncomeRequirement,
                    Size = post.PostDetail.Size,
                    NearbySchools = post.PostDetail.NearbySchools,
                    NearbyBusStops = post.PostDetail.NearbyBusStops,
                    NearbyRestaurants = post.PostDetail.NearbyRestaurants
                } : null;

                return new PostResponseDTO
                {
                    Id = post.Id,
                    Title = post.Title,
                    Price = post.Price,
                    Address = post.Address,
                    City = post.City,
                    Bedroom = post.Bedroom,
                    Bathroom = post.Bathroom,
                    Latitude = post.Latitude,
                    Longitude = post.Longitude,
                    Type = post.Type,
                    Property = post.Property,
                    CreatedAt = post.CreatedAt,
                    UserId = post.UserId,
                    User = userDTO,
                    PostDetail = postDetailDTO
                };
            }).ToList();

            return postDTOs;
        }

    }
}
