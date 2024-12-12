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
        public async Task<ActionResult<IEnumerable<PostResponseDTO>>> GetAllPosts()
        {
            try
            {
                // Lấy danh sách bài đăng với chi tiết người dùng và bài đăng
                var posts = await _postService.GetAllPostWithDetailAsync();

                // Kiểm tra nếu danh sách rỗng
                if (posts == null || !posts.Any())
                {
                    return NoContent(); // Nếu không có bài đăng, trả về NoContent
                }

                return Ok(posts); // Trả về danh sách bài đăng với chi tiết
            }
            catch (Exception ex)
            {
                // Log lỗi (nếu có)
                _logger.LogError(ex, "An error occurred while fetching posts.");

                // Trả về mã lỗi 500 nếu có ngoại lệ
                return StatusCode(500, new { message = "An error occurred while fetching posts.", details = ex.Message });
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
               
                await _postService.AddPostAsync(postDTO, userId);
                return Ok(new
                {
                    status = "success",
                    message = "Post created successfully"
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
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(string id)
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


                var isDeleted = await _postService.DetelePostAsync(id, userId);
                if (!isDeleted)
                {
                    return BadRequest(new
                    {
                        status = "error",
                        message = "Failed to delete post."
                    });
                }

                return Ok(new
                {
                    status = "success",
                    message = "Post deleted successfully"
                });
                
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new
                {
                    status = "error",
                    message = ex.Message
                });
            }
        } 


    }
}
