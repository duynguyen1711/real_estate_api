using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using real_estate_api.DTOs;
using real_estate_api.Interface.Service;
using System.Security.Claims;

namespace real_estate_api.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostController: ControllerBase
    {
        private readonly IPostService _postService;
        private readonly ILogger<PostController> _logger;
        public PostController(IPostService postService, ILogger<PostController> logger)
        {
            _postService = postService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPost() { 
            try
            {
                var posts = await _postService.GetAllPostAsync();
                return Ok(posts);
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
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreatePost(PostCreateDTO postDTO)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return Unauthorized(new
                    {
                        status = "error",
                        message = "User is not authorized."
                    });
                }
                postDTO.UserId = userId;
                var createdPost = await _postService.AddPostAsync(postDTO);
                return Ok(new
                {
                    status = "success",
                    message = "Post created successfully",
                    data = createdPost
                });
            }
            catch (ApplicationException ex)
            {
                // Xử lý lỗi logic của ứng dụng (400 Bad Request)
                return BadRequest(new
                {
                    status = "error",
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi không mong muốn (500 Internal Server Error)
                return StatusCode(500, new
                {
                    status = "error",
                    message = "An unexpected error occurred.",
                    details = ex.Message
                });
            }
        }
    }
}
