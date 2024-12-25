using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using real_estate_api.DTOs;
using real_estate_api.Interface.Service;
using System.Security.Claims;

namespace real_estate_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }
        [Authorize]
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
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{chatId}")]
        public async Task<IActionResult> GetChat(string chatId)
        {
            var chat = await _chatService.GetChatAsync(chatId);
            if (chat == null)
            {
                return NotFound();
            }
            return Ok(chat);
        }

        [HttpGet]
        public async Task<IActionResult> GetChats()
        {
            var chats = await _chatService.GetChatsAsync();
            return Ok(chats);
        }
    }
}
