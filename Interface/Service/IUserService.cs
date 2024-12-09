using real_estate_api.DTOs;
using real_estate_api.Models;

namespace real_estate_api.Interface.Service
{
    public interface IUserService
    {
        Task<UserDTO> GetUserByIdAsync(string id);
        Task<UserDTO> GetUserByEmailAsync(string email);
        Task<UserDTO> GetUserByUsernameAsync(string username);
        Task<UserDTO> UpdateUserAsync(UpdateUserRqDTO userDTO,string id);
        Task<bool> DeleteUserAsync(string id);
        Task<List<UserDTO>> GetAllUsersAsync();
    }
}
