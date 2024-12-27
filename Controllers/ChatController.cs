using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using real_estate_api.DTOs;
using real_estate_api.Interface.Service;
using System.Security.Claims;

namespace real_estate_api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/chats")]
    
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }
        
        [HttpPost("create")]
        public async Task<IActionResult> CreateChat([FromBody] CreateChatRequest request)
        {
            var currentUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (request.UserId == null)
            {
                return Unauthorized(new
                {
                    status = "error",
                    message = "User is not authorized."
                });
            }
            if (request.UserId == currentUserId)
            {
                return BadRequest(new
                {
                    status = "error",
                    message = "You cannot create a chat with yourself."
                });
            }
            var userIds = new List<string> { request.UserId, currentUserId };
            try
            {
                var chat = await _chatService.CreateChatAsync(userIds);
                return Ok(new { chat });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    status = "error",
                    message = ex.Message
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    status = "error",
                    message = ex.Message
                });
            }
        }

        [HttpGet("{chatId}")]
        public async Task<IActionResult> GetChat(string chatId)
        {
            var currentUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == null)
            {
                return Unauthorized(new
                {
                    status = "error",
                    message = "User is not authorized."
                });
            }
            try
            {
                var chat = await _chatService.GetChatAsync(chatId, currentUserId);
                if (chat == null)
                {
                    return NotFound(new
                    {
                        status = "error",
                        message = "Chat not found or you are not authorized to access it."
                    });
                }
                return Ok(chat);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    status = "error",
                    message = ex.Message
                });
            }      
        }

        [HttpGet]
        public async Task<IActionResult> GetChats()
        {
            var currentUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == null)
            {
                return Unauthorized(new
                {
                    status = "error",
                    message = "User is not authorized."
                });
            }
            var chats = await _chatService.GetChatsAsync(currentUserId);
            await Task.Delay(3000);
            return Ok(chats);
        }
        [HttpPut("read/{chatId}")]
        public async Task<IActionResult> MarkChatAsRead(string chatId)
        {
            // Lấy userId từ JWT token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User is not authenticated." });
            }

            try
            {
                // Gọi service để xử lý
                await _chatService.MarkChatAsReadAsync(chatId, userId);

                return Ok(new { 
                    message = "Chat marked as read successfully.",
                    status = "success"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
