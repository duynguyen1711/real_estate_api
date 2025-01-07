using AutoMapper;
using real_estate_api.DTOs;
using real_estate_api.Helpers;
using real_estate_api.Interface.Service;
using real_estate_api.Models;
using real_estate_api.UnitofWork;
using System;
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
       

        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new ApplicationException("User not found");
            }
            await _unitOfWork.UserRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
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

        public async Task<UserDTO> UpdateUserAsync(UpdateUserRqDTO userDTO,string id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new ApplicationException("User not found");
            }
            var duplicateUsers = await _unitOfWork.UserRepository.FindDuplicatesAsync(userDTO.Username, userDTO.Email);
            if (duplicateUsers.Any(u => u.Username == userDTO.Username && u.Id != user.Id))
            {
                throw new ApplicationException("Username already exists");
            }
            if (duplicateUsers.Any(u => u.Email == userDTO.Email && u.Id != user.Id))
            {
                throw new ApplicationException("Email already exists");
            }


            user.Username = !string.IsNullOrWhiteSpace(userDTO.Username) ? userDTO.Username : user.Username;
            user.Password = !string.IsNullOrWhiteSpace(userDTO.Password) ? PasswordHelper.HashPassword(userDTO.Password) : user.Password;
            user.Email = !string.IsNullOrWhiteSpace(userDTO.Email) ? userDTO.Email : user.Email;
            user.Avatar = !string.IsNullOrWhiteSpace(userDTO.Avatar) ? userDTO.Avatar : user.Avatar;
            user.UpdatedAt = DateTime.Now;
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserDTO>(user);
        }
        public async Task<int> GetUnreadMessagesCountAsync(string userId)
        {
            // Lấy tất cả các chat của user
            var chatList = await _unitOfWork.ChatRepository.GetChatsByUserAsync(userId);

            // Lọc ra các tin nhắn chưa được xem
            var unreadMessagesCount = chatList
                .Where(chat => chat.SeenByUsers
                    .Any(s => s.UserId == userId && s.IsSeen == false))
                .Count();

            return unreadMessagesCount;
        }
    }
}
