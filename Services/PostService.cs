using AutoMapper;
using real_estate_api.DTOs;
using real_estate_api.Helpers;
using real_estate_api.Interface.Service;
using real_estate_api.Models;
using real_estate_api.UnitofWork;

namespace real_estate_api.Services
{
    public class PostService: IPostService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PostService(IUnitOfWork unitOfWork, IMapper mapper )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PostResponseDTO> AddPostAsync(PostCreateDTO postDTO)
        {
            try
            {
                var post = _mapper.Map<Post>(postDTO);
                await _unitOfWork.PostRepository.AddAsync(post);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<PostResponseDTO>(post);
            }
            catch( Exception ex ) {
                throw new Exception("An error occurred while adding the post.");
            }
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
    }
}
