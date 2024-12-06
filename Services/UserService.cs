using AutoMapper;
using real_estate_api.DTOs;
using real_estate_api.Helpers;
using real_estate_api.Interface.Service;
using real_estate_api.Models;
using real_estate_api.UnitofWork;
using System.Collections.Generic;

namespace real_estate_api.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserService( IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            
        }
       

        public Task<bool> DeleteUserAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            var userList = await _unitOfWork.UserRepository.GetAllAsync();
            return _mapper.Map< List < UserDTO >>(userList);
        }

        public async Task<UserDTO> GetUserByEmailAsync(string email)
        {
            var user = await _unitOfWork.UserRepository.GetByEmailAsync(email);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> GetUserByIdAsync(string id)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
                return _mapper.Map<UserDTO>(user);

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occurred while getting user by id.", ex);
            }
            
        }

        public async Task<UserDTO> GetUserByUsernameAsync(string username)
        {
            var user = await _unitOfWork.UserRepository.GetByUsernameAsync(username);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<User> UpdateUserAsync(UpdateUserDTO userDTO,string id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if(user == null)
            {
                throw new ApplicationException("User not found");
            }
            if (!string.IsNullOrEmpty(userDTO.Password))
            {
                userDTO.Password = PasswordHelper.HashPassword(userDTO.Password);
            }       
            _mapper.Map(userDTO, user);
            await _unitOfWork.UserRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<User>(user);
        }
    }
}
