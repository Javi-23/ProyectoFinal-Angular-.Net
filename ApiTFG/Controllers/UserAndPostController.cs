using ApiTFG.Dtos;
using ApiTFG.Services.UserAndPostService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiTFG.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAndPostController : ControllerBase
    {
        private readonly IUserAndPostService _userAndPostService;

        public UserAndPostController(IUserAndPostService userAndPostService)
        {
            _userAndPostService = userAndPostService;
        }

        [HttpGet("posts/{username}")]
        public async Task<ActionResult<UserAndPostDto>> GetUserPosts(string username)
        {
            try
            {
                var userAndPostDto = await _userAndPostService.GetUserPosts(username);
                return Ok(userAndPostDto);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener las publicaciones del usuario: {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("posts/all")]
        public async Task<ActionResult<List<UserAndPostDto>>> GetAllPosts()
        {
            try
            {
                var userAndPostDto = await _userAndPostService.GetAllPosts();
                return Ok(userAndPostDto);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener las publicaciones del usuario");
            }
        }

        [Authorize]
        [HttpGet("posts/followed")]
        public async Task<ActionResult<List<UserAndPostDto>>> GetFollowedPosts()
        {
            try
            {
                var userAndPostDto = await _userAndPostService.GetFollowedPosts();
                return Ok(userAndPostDto);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener las publicaciones de los seguidos del usuario: {ex.Message}");
            }
        }
    }
}
