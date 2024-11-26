using real_estate_api.Models;

namespace real_estate_api.Interface.Repository
{
    public interface IUserRepository
    {
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByEmailAsync(string email);
        Task AddAsync(User user);    }
}
