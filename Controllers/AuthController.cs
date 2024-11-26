using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using real_estate_api.DTOs;
using real_estate_api.Interface.Service;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace real_estate_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO userDto)
        {
            try
            {
                await _authService.RegisterAsync(userDto);
                return Ok(new { message = "User registered successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDTO login)
        {
            try
            {
                // Gọi phương thức đăng nhập để lấy JWT token
                var loginResponse = await _authService.LoginAsync(login);
                var jwtToken = loginResponse.Token;

                // Cấu hình các tùy chọn cookie
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,  // Đảm bảo cookie chỉ có thể được truy cập bởi server, không qua JavaScript
                    Secure = true,    // Cookie chỉ được gửi qua HTTPS. Đảm bảo môi trường của bạn đang sử dụng HTTPS
                    SameSite = SameSiteMode.Strict,  // Chế độ bảo vệ cho cookie, tránh bị lộ khi gửi request từ domain khác
                    Expires = DateTime.UtcNow.AddDays(1)  // Thời gian hết hạn của cookie, 1 ngày trong trường hợp này
                };

                // Xóa cookie cũ nếu có
                Response.Cookies.Delete("auth_token");

                // Lưu JWT token vào cookie
                Response.Cookies.Append("auth_token", jwtToken, cookieOptions);

                // Trả về response cho client
                return Ok(loginResponse);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi khi login không thành công
                return Unauthorized(new { message = ex.Message });
            }
        }


    }
}
