
using ApiTFG.NewFolder;
using ApiTFG.Services.UserToFollow;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiTFG.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserToFollowController : ControllerBase
    {
        private readonly IUserToFollowService _userToFollowService;

        public UserToFollowController(IUserToFollowService userToFollowService)
        {
            _userToFollowService = userToFollowService;
        }

        [HttpPost("follow-user/{username}")]
        public async Task<ActionResult> FollowUser(string username)
        {
            try
            {
                var result = await _userToFollowService.FollowUser(username);

                if (!result)
                {
                    return BadRequest("No se pudo realizar la operación");
                }

                return Ok("Usuario seguido correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("unfollow-user/{username}")]
        public async Task<ActionResult> UnfollowUser(string username)
        {
            try
            {
                var result = await _userToFollowService.UnfollowUser(username);

                if (!result)
                {
                    return BadRequest("No se puede dejar de seguir a ti mismo | No estás siguiendo a este usuario");
                }

                return Ok("Se dejo de seguir al usuario");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("followed-users/{username}")]
        public async Task<ActionResult<List<UserViewModel>>> GetFollowedUsers(string username)
        {
            try
            {
                var followedUsers = await _userToFollowService.GetFollowedUsers(username);
                return Ok(followedUsers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("follower-users/{username}")]
        public async Task<ActionResult<List<UserViewModel>>> GetFollowerUsers(string username)
        {
            try
            {
                var followedUsers = await _userToFollowService.GetFollowerUsers(username);
                return Ok(followedUsers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}