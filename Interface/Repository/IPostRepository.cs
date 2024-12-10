using real_estate_api.Models;

namespace real_estate_api.Interface.Repository
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetAllAsync();
        Task AddAsync(Post post);
        Task UpdateAsync(Post post);
        Task<bool> DeleteAsync(string id);
    }
}
