using ApiTFG.Dtos;
using ApiTFG.Models;
using ApiTFG.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiTFG.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register", Name = "RegisterUser")]
        public async Task<IActionResult> RegisterUser(LoginUser user)
        {
            if (await _authService.RegisterUser(user))
            {
                return Ok("Successfully registered");
            }
            return BadRequest("Something went wrong");
        }

        [HttpPost("Login", Name = "LoginUser")]
        public async Task<IActionResult> Login(LoginDTO user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (await _authService.Login(user))
            {
                var tokenString = _authService.GenerateTokenString(user);
                return Ok(new LoginRequestResponse()
                {
                    Token = tokenString,
                    Result = true,
                });
            }
            return BadRequest();
        }
    }
}