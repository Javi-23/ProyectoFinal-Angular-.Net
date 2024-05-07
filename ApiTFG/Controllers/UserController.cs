using ApiTFG.Services.User;
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

        [HttpGet("{username}")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            var userViewModel = await _userService.GetUserByUsernameAsync(username);
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
    }
}