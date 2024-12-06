using real_estate_api.Models;

namespace real_estate_api.Interface.Repository
{
    public interface IUserRepository
    {
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByIdAsync(string id);
        Task<IEnumerable<User>> GetAllAsync();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task <bool>DeleteAsync(string id);

    }
}
