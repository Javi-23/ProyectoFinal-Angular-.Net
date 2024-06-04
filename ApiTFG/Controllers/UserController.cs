using ApiTFG.Services.User;
using ApiTFG.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiTFG.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet("{username}")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            var userViewModel = await _userService.GetUserByUserNameAsync(username);
            if (userViewModel == null)
            {
                return NotFound();
            }

            return Ok(userViewModel);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetMyProfile()
        {
            var userViewModel = await _userService.GetMyProfile();
            if (userViewModel == null)
            {
                return NotFound();
            }

            return Ok(userViewModel);
        }

        [Authorize]
        [HttpPut("update-description")]
        public async Task<IActionResult> UpdateUserDescription([FromBody] string newDescription)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var updatedUser = await _userService.UpdateUserDescriptionAsync(token, newDescription);
                return Ok(updatedUser);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("usersByPrefix")]
        public async Task<IActionResult> GetUsersByPrefix([FromQuery] string prefix)
        {
            try
            {
                var users = await _userService.GetUsersByUsernameStartingWithAsync(prefix);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("upload-image")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile imageFile)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var updatedUser = await _userService.UpdateUserImageAsync(token, imageFile);
                return Ok(updatedUser);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("download-uploaded-image")]
        public async Task<IActionResult> DownloadUploadedImage([FromQuery] string username)
        {
            try
            {
                var user = await _userService.GetUserByUserNameAsync(username);
                byte[] imageBytes = user.Image;

                return File(imageBytes, "image/jpeg", $"{username}_uploaded_image.jpg");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}