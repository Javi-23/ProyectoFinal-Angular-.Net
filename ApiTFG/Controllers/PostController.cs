using ApiTFG.Dtos;
using ApiTFG.Models;
using ApiTFG.Requests;
using ApiTFG.Services;
using ApiTFG.Services.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiTFG.Controllers
{
    [Authorize] // Requiere autenticación para acceder a este controlador
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpPost("create-post")]
        public async Task<ActionResult<PostDTO>> CreatePost([FromForm] CreateUpdatePostRequest request)
        {
            try
            {
                var createdPost = await _postService.CreatePost(request.Text, request.ImageFile);
                return Ok(createdPost);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-post/{id}")]
        public async Task<ActionResult<PostDTO>> UpdatePost(int id, [FromBody] CreateUpdatePostRequest request)
        {
            try
            {
                var result = await _postService.UpdatePost(id, request.Text);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-post/{id}")]
        public async Task<ActionResult<bool>> DeletePost(int id)
        {
            try
            {
                var result = await _postService.DeletePost(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create-comment/{id}")]
        public async Task<ActionResult<PostDTO>> CreateComment(int id,[FromBody] string text)
        {
            try
            {
                var createdPost = await _postService.CreateComment(id, text);
                return Ok(createdPost);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-comment/{id}")]
        public async Task<ActionResult<bool>> DeleteComment(int id)
        {
            try
            {
                var result = await _postService.DeleteComment(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("like-post/{id}")]
        public async Task<ActionResult<bool>> LikePost(int id)
        {
            try
            {
                var result = await _postService.LikePost(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("download-uploaded-image")]
        public async Task<IActionResult> DownloadUploadedImage([FromQuery] int postId)
        {
            try
            {
                var imageBytes = await _postService.GetPostImage(postId);

                if (imageBytes == null || imageBytes.Length == 0)
                {
                    return NotFound("Image not found for the specified post.");
                }

                return File(imageBytes, "image/jpeg", $"{postId}_uploaded_image.jpg");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}