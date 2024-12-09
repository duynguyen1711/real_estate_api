using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using real_estate_api.DTOs;
using real_estate_api.Helpers;
using real_estate_api.Interface.Service;
using real_estate_api.Models;
using real_estate_api.Services;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Security.Claims;

namespace real_estate_api.Controllers
{
   
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            if (users == null || !users.Any())
            {
                return NotFound(new { message = "No users found." });
            }

            // Trả về danh sách người dùng
            return Ok(users);
        }
        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }
                return Ok(user);
            }
            catch (Exception ex)
            { 
                _logger.LogError(ex, "An error occurred while retrieving user by email.");
                return StatusCode(500, "An unexpected error occurred.");
            }



        }
        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUserByEmail([EmailAddress(ErrorMessage = "Invalid email format.")] string email)
        {
            try
            {
                var user = await _userService.GetUserByEmailAsync(email);
                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                return Ok(user);
            }catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving user by email.");
                return StatusCode(500, "An unexpected error occurred.");
            }
            
        }
        [HttpGet("username/{username}")]
        public async Task<IActionResult> GetUserByName(string username)
        {
            var user = await _userService.GetUserByUsernameAsync(username);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            return Ok(user);
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRqDTO userDTO, string id)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (userId != id)
                {
                    return Forbid();
                }
                var updatedUser = await _userService.UpdateUserAsync(userDTO, userId);
                if (updatedUser == null)
                {
                    return NotFound(new { message = "User not found or update failed." });
                }

                return Ok(updatedUser);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            { 
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                return Ok("Deleted");
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


    }
}
