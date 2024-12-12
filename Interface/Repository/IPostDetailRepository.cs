using real_estate_api.Models;

namespace real_estate_api.Interface.Repository
{
    public interface IPostDetailRepository
    {
        Task<PostDetail> UpdateAsync(PostDetail postDetail);
    }
}
