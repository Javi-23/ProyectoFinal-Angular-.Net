using Microsoft.AspNetCore.Http;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace ApiTFG.Utils
{
    public static class JwtUtils
    {
        public static string ExtractJwtToken(HttpContext httpContext)
        {
            var jwtToken = httpContext.Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");

            if (string.IsNullOrEmpty(jwtToken))
            {
                throw new ArgumentException("No se encontró ningún token JWT en el encabezado de autorización.");
            }

            return jwtToken;
        }

        public static string ExtractUsernameFromToken(string jwtToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(jwtToken) as JwtSecurityToken;

            var username = jsonToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
            if (username == null)
            {
                throw new ArgumentException("El token JWT no contiene información del usuario.");
            }

            return username;
        }

        public static string ExtractUserIdFromToken(string jwtToken)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(jwtToken) as JwtSecurityToken;

                var userId = jsonToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    throw new ArgumentException("El token JWT no contiene información del usuario.");
                }

                return userId;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}