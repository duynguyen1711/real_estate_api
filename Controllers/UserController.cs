using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using real_estate_api.DTOs;
using real_estate_api.Models;
using System.Security.Claims;

namespace real_estate_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [Authorize]
        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            // Lấy userId từ claim NameIdentifier
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Kiểm tra nếu không có userId trong claim
            if (userId == null)
            {
                return Unauthorized(new { message = "User ID not found in claims." });
            }

            // Bạn có thể trả về userId hoặc thực hiện các xử lý khác ở đây
            return Ok(new { UserId = userId });
        }
    }
}
