using real_estate_api.DTOs;
using real_estate_api.Models;

namespace real_estate_api.Interface.Service
{
    public interface ISavedPostService
    {
        Task AddSavedPostAsync(SavedPostDTO savedPostDTO,string userId);
        Task<SavedPost> FindSavedPost(string userId, string postId);
        Task<bool> DeleteSavedPost(string userId, string postId);

    }
}
