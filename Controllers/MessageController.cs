using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using real_estate_api.DTOs;
using real_estate_api.Interface.Service;
using System.Security.Claims;

namespace real_estate_api.Controllers
{
    [Route("api/messages")]
    [ApiController]
    [Authorize]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddMessage([FromBody] MessageRequestDTO messageRequestDTO)
        {
            var currentUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == null) {
                {
                    return BadRequest(new
                    {
                        status = "error",
                        message = "User not authorize"
                    });
                }
            }
            if (messageRequestDTO == null)
            {
                return BadRequest(new
                {
                    status = "error",
                    message = "Invalid request."
                });
            }

            try
            {
                var responseDTO = await _messageService.AddMessageAsync(messageRequestDTO, currentUserId);

                return Ok(new
                {
                    status = "success",
                    message = "Message added successfully.",
                    data = responseDTO
                });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new
                {
                    status = "error",
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "error",
                    message = "An error occurred while adding the message.",
                    details = ex.Message
                });
            }
        }
    }
}
