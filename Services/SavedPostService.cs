using AutoMapper;
using real_estate_api.DTOs;
using real_estate_api.Interface.Service;
using real_estate_api.Models;
using real_estate_api.UnitofWork;

namespace real_estate_api.Services
{
    public class SavedPostService : ISavedPostService

    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SavedPostService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task AddSavedPostAsync(SavedPostDTO savedPostDTO, string userId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(savedPostDTO.UserId);
            if (user == null)
            {
                throw new ApplicationException("User does not exist.");
            }

            if (savedPostDTO.UserId != userId)
            {
                throw new ApplicationException("User not authorized.");
            }

            var post = await _unitOfWork.PostRepository.GetPost(savedPostDTO.PostId);
            if (post == null)
            {
                throw new ApplicationException("Post does not exist.");
            }
            var existingSavedPost = await FindSavedPost(savedPostDTO.UserId, savedPostDTO.PostId);

            if (existingSavedPost != null)
            {
                throw new ApplicationException("Post has already been saved.");
            }
            var savedPost = _mapper.Map<SavedPost>(savedPostDTO);
            await _unitOfWork.SavedPostRepository.AddSavedPost(savedPost);

            // Lưu các thay đổi trong UnitOfWork
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> DeleteSavedPost(string userId, string postId)
        {
            var existingSavedPost = await FindSavedPost(userId, postId);

            if (existingSavedPost == null)
            {
                throw new ApplicationException("Post haven't been saved.");
            }
            await _unitOfWork.SavedPostRepository.DeleteSavedPost(existingSavedPost);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<SavedPost> FindSavedPost(string userId, string postId)
        {
            var existingSavedPost = await _unitOfWork.SavedPostRepository
                .GetSavedPost(userId, postId); 
            return existingSavedPost;
        }

        public async Task<List<SavedPostDTOResponse>> GetListSavedPostOfUser(string userId)
        {
            var user = await  _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException("User not found");
            }
            var postList = await _unitOfWork.SavedPostRepository.GetListSavedPostOfUser(userId);

            // Nếu không có bài viết đã lưu
            if (postList == null || !postList.Any())
            {
                throw new ApplicationException("No saved posts found for the user");
            }

            // Ánh xạ SavedPost sang SavedPostDTOResponse
            var postListDTO = postList.Select(post => new SavedPostDTOResponse
            {
                Id = post.Post.Id,
                Title = post.Post.Title,
                Price = post.Post.Price,
                Address = post.Post.Address,
                Bedroom = post.Post.Bedroom,
                Bathroom = post.Post.Bathroom,
            }).ToList();

            return postListDTO;
        }
    }
}
