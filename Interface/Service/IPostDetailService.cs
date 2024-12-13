using real_estate_api.DTOs;
using real_estate_api.Models;

namespace real_estate_api.Interface.Service
{
    public interface IPostDetailService
    {
        Task UpdatePostDetail(Post post, PostDetailUpdateDTO postDetailDTO);
    }
}
