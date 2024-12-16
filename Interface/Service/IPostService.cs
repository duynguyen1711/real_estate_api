using real_estate_api.DTOs;
using real_estate_api.Models;

namespace real_estate_api.Interface.Service
{
    public interface IPostService
    {
        Task AddPostAsync(PostCreateDTO postDTO,string id);
        Task<IEnumerable<PostResponseDTO>> GetAllPostAsync();
        Task<IEnumerable<PostResponseDTO>> GetAllPostWithDetailAsync(Query query);
        Task<bool> UpdatePostAsync(PostUpdateDTO postUpdateDTO, string userId,string postId);
        Task<bool> DetelePostAsync(string id,string userId);
        Task<PostResponseDTO> GetPostWithDetail(string postId);
        Task<List<PostResponseDTO>> GetPostOfUserAsync(string userId);
    }
}
