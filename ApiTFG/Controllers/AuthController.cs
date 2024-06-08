using ApiTFG.Dtos;
using ApiTFG.Models;
using ApiTFG.Requests;
using ApiTFG.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApiTFG.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAuthService _authService;

        public AuthController(UserManager<AppUser> userManager, IAuthService authService)
        {
            _userManager = userManager;
            _authService = authService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] LoginUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.RegisterUser(user);

            if (result.Succeeded)
            {
                return Ok(new { message = "Usuario registrado con éxito." });
            }

            var errors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(new { errors });
        }

        [HttpPost("Login", Name = "LoginUser")]
        public async Task<IActionResult> Login(LoginDTO user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var identityUser = await _userManager.FindByNameAsync(user.UserName);
            if (identityUser is null)
            {
                return BadRequest(new { message = "El usuario no existe." });
            }

            if (!await _userManager.CheckPasswordAsync(identityUser, user.Password))
            {
                return BadRequest(new { message = "Contraseña incorrecta." });
            }

            var tokenString = await _authService.GenerateTokenString(user);
            return Ok(new LoginRequestResponse()
            {
                Token = tokenString,
                Result = true,
            });
        }


        [HttpPost("ValidateToken", Name = "ValidateToken")]
        public async Task<IActionResult> ValidateToken([FromBody] TokenRequestModel model)
        {
            if (await _authService.IsTokenValid(model.Token))
            {
                return Ok(new { Result = true });
            }
            return BadRequest(new { Result = false, Message = "Invalid token" });
        }
    }
}