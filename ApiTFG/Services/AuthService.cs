using ApiTFG.Dtos;
using ApiTFG.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApiTFG.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _config;
        private static List<string> revokedTokens = new List<string>();

        public AuthService(UserManager<AppUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

        public async Task<bool> RegisterUser(LoginUser user)
        {
            var appUser = new AppUser
            {
                UserName = user.UserName,
                Email = user.Email,
                Description = !string.IsNullOrEmpty(user.Description) ? user.Description : null,
            };

            var result = await _userManager.CreateAsync(appUser, user.Password);
            return result.Succeeded;
        }

        public async Task<bool> Login(LoginDTO user)
        {
            var identityUser = await _userManager.FindByNameAsync(user.UserName);
            if (identityUser is null)
            {
                return false;
            }

            return await _userManager.CheckPasswordAsync(identityUser, user.Password);
        }

        public async Task<string> GenerateTokenString(LoginDTO user)
        {
            var identityUser = await _userManager.FindByNameAsync(user.UserName);
            if (identityUser == null)
            {
                throw new ApplicationException("Usuario no encontrado");
            }

            var userId = identityUser.Id;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.UserName), 
                new Claim(ClaimTypes.NameIdentifier, userId), 
                new Claim(ClaimTypes.Role, "Admin"),
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));
            var signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                issuer: _config.GetSection("Jwt:Issuer").Value,
                audience: _config.GetSection("Jwt:Audience").Value,
                signingCredentials: signingCred);

            string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return tokenString;

        }

        public async Task<bool> IsTokenValid(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _config.GetSection("Jwt:Issuer").Value,
                    ValidateAudience = true,
                    ValidAudience = _config.GetSection("Jwt:Audience").Value,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
