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
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _logger.LogInformation("Registration attempt for user: {Username}", userDto.Username);
                await _authService.RegisterAsync(userDto);
                _logger.LogInformation("User registered successfully: {Username}", userDto.Username);
                return Ok(new { message = "User registered successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during registration for user: {Username}", userDto.Username);
                return BadRequest(new { error = ex.Message });
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDTO login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                // Gọi phương thức đăng nhập để lấy JWT token
                var loginResponse = await _authService.LoginAsync(login);
                var jwtToken = loginResponse.Token;

                // Cấu hình các tùy chọn cookie
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,  // Đảm bảo cookie chỉ có thể được truy cập bởi server, không qua JavaScript
                    Secure = false,    
                    SameSite = SameSiteMode.Strict,  // Chế độ bảo vệ cho cookie, tránh bị lộ khi gửi request từ domain khác
                    Expires = DateTime.UtcNow.AddDays(1)  // Thời gian hết hạn của cookie, 1 ngày trong trường hợp này
                };

                // Xóa cookie cũ nếu có
                Response.Cookies.Delete("auth_token");

                // Lưu JWT token vào cookie
                Response.Cookies.Append("auth_token", jwtToken, cookieOptions);

                // Trả về response cho client
                return Ok( new LoginResponseDTO 
                { 
                    Id = loginResponse.Id,
                    Username = loginResponse.Username,
                    Avatar = loginResponse.Avatar,
                    Message = loginResponse.Message,
                    Email = loginResponse.Email,
                }
                );
            }
            catch (Exception ex)
            {
                // Xử lý lỗi khi login không thành công
                return Unauthorized(new { message = ex.Message });
            }
        }
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            try
            {
                // Xóa cookie chứa JWT
                Response.Cookies.Delete("auth_token");

                return Ok(new { message = "Logout successful" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


    }
}
