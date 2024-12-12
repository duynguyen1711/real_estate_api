using real_estate_api.DTOs;

namespace real_estate_api.Interface.Service
{
    public interface IPostService
    {
        Task AddPostAsync(PostCreateDTO postDTO,string id);
        Task<IEnumerable<PostResponseDTO>> GetAllPostAsync();
        Task<IEnumerable<PostResponseDTO>> GetAllPostWithDetailAsync();
        Task<bool> UpdatePostAsync(PostUpdateDTO postUpdateDTO, string id);
        Task<bool> DetelePostAsync(string id,string userId);
    }
}
