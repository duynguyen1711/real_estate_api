using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using real_estate_api.DTOs;
using real_estate_api.Interface.Service;
using System.Security.Claims;

namespace real_estate_api.Controllers
{
    [Route("api/saved-posts")]
    [ApiController]
    public class SavedPostController : ControllerBase
    {
        private readonly ISavedPostService _savedPostService;
        private readonly ILogger<SavedPostController> _logger;
        public SavedPostController(ISavedPostService savedPostService, ILogger<SavedPostController> logger)
        {
            _savedPostService = savedPostService;
            _logger = logger;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddSavedPost(SavedPostDTO savePostDTO)
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
            try
            {
                await _savedPostService.AddSavedPostAsync(savePostDTO, userId);
                return Ok(new
                {
                    status = "success",
                    message = "Post has been saved successfully."
                });
            }
            catch (ApplicationException appEx)
            {
                _logger.LogError(appEx, "Application error occurred while saving post");

                return BadRequest(new
                {
                    status = "error",
                    message = appEx.Message // Trả về thông báo lỗi từ ApplicationException
                });
            }
            catch (Exception ex)
            {
                // Log tất cả các lỗi khác (cơ sở dữ liệu, hệ thống, v.v.)
                _logger.LogError(ex, "Unexpected error occurred while saving post");

                // Trả về lỗi chung
                return StatusCode(500, new
                {
                    status = "error",
                    message = "An error occurred while processing your request."
                });
            }
        }
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteSavedPost(string postId)
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
            try
            {
                var existingSavedPost = await _savedPostService.FindSavedPost(userId, postId);
                if (existingSavedPost == null)
                {
                    return NotFound(new
                    {
                        status = "error",
                        message = "Saved post not found."
                    });
                }
                if (existingSavedPost.UserId != userId)
                {
                    return Unauthorized(new
                    {
                        status = "error",
                        message = "You are not authorized to delete this post."
                    });
                }
                await _savedPostService.DeleteSavedPost(userId, postId);
                return Ok(new
                {
                    status = "success",
                    message = "Post has been deleted successfully."
                });
            }
            catch (ApplicationException appEx)
            {
                _logger.LogError(appEx, "Application error occurred while deleting post");

                return BadRequest(new
                {
                    status = "error",
                    message = appEx.Message // Trả về thông báo lỗi từ ApplicationException
                });
            }
            catch (Exception ex)
            {
                // Log tất cả các lỗi khác (cơ sở dữ liệu, hệ thống, v.v.)
                _logger.LogError(ex, "Unexpected error occurred while saving post");

                // Trả về lỗi chung
                return StatusCode(500, new
                {
                    status = "error",
                    message = "An error occurred while processing your request."
                });
            }
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetSavedPostOfUser()
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
            try
            {
                var post = await _savedPostService.GetListSavedPostOfUser(userId);
                return Ok(post);
            }
            catch (ApplicationException appEx) {
                _logger.LogError(appEx, "Application error occurred while fetch post");

                return BadRequest(new
                {
                    status = "error",
                    message = appEx.Message // Trả về thông báo lỗi từ ApplicationException
                });
            }
        }
    }
}
   
