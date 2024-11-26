using real_estate_api.DTOs;

namespace real_estate_api.Interface.Service
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterRequestDTO userDto);
        Task<LoginResponseDTO> LoginAsync(LoginRequestDTO login);
    }
}
