using real_estate_api.DTOs;
using real_estate_api.Models;

namespace real_estate_api.Interface.Repository
{
    public interface ISavedPostRepository
    {
        Task AddSavedPost(SavedPost savedPost);
        Task DeleteSavedPost(SavedPost savedPost);
        Task<SavedPost> GetSavedPost(string userID, string PostId);
        Task<List<SavedPost>> GetListSavedPostOfUser(string userID);
    }
}
