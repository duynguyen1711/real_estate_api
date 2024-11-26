using AutoMapper;
using Microsoft.AspNetCore.Identity;
using real_estate_api.DTOs;
using real_estate_api.Helpers;
using real_estate_api.Interface.Service;
using real_estate_api.Models;
using real_estate_api.UnitofWork;

namespace real_estate_api.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AuthService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<bool> RegisterAsync(UserRegisterDTO userDto)
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
    }
}
