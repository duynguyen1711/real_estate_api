using real_estate_api.DTOs;

namespace real_estate_api.Interface.Service
{
    public interface IPostService
    {
        Task<PostResponseDTO> AddPostAsync(PostCreateDTO postDTO);
        Task<IEnumerable<PostResponseDTO>> GetAllPostAsync();
    }
}
