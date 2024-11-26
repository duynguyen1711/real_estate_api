using AutoMapper;
using Microsoft.AspNetCore.Identity;
using real_estate_api.DTOs;
using real_estate_api.Helpers;
using real_estate_api.Interface.Service;
using real_estate_api.Models;
using real_estate_api.UnitofWork;
using System.IdentityModel.Tokens.Jwt;

namespace real_estate_api.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly JwtHelper _jwtHelper;

        public AuthService(JwtHelper jwtHelper,IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jwtHelper = jwtHelper;
        }

        public async Task<bool> RegisterAsync(RegisterRequestDTO userDto)
        {
            var existingUserByUsername = await _unitOfWork.UserRepository.GetByUsernameAsync(userDto.Username);
            if (existingUserByUsername != null)
                throw new Exception("Username already exists.");
            var existingUserByEmail = await _unitOfWork.UserRepository.GetByEmailAsync(userDto.Email);
            if (existingUserByEmail != null)
                throw new Exception("Username already exists");
     
            var user = _mapper.Map<User>(userDto);
            user.Password = PasswordHelper.HashPassword(user.Password);

            await _unitOfWork.UserRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO login)
        {
            var existingUserByUsername = await _unitOfWork.UserRepository.GetByUsernameAsync(login.Username);
            if (existingUserByUsername == null)
                throw new Exception("Invalid credentials");

            // Kiểm tra mật khẩu
            var isPasswordValid = PasswordHelper.VerifyPassword(login.Password, existingUserByUsername.Password);
            if (!isPasswordValid)
            {
                throw new Exception("Invalid credentials");
            }

            // Tạo token JWT
            var token = _jwtHelper.GenerateJwtToken(existingUserByUsername.Username, existingUserByUsername.Id);

            // Trả về LoginResponseDTO không có password và có token
            return new LoginResponseDTO
            {
                Message = "Login successful.",
                Token = token,
                UserId = existingUserByUsername.Id,
                Username = existingUserByUsername.Username
              
            };
        }

    }
}
