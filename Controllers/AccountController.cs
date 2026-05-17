using KhadeFarm_Web_API.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace KhadeFarm_Web_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginRequest request)
        {
            // 1. Validate credentials (normally from DB)
            if (!IsValidUser(request.UserName, request.Password))
            {
                return Unauthorized("Invalid username or password");
            }

            // 2. Create claims
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, request.UserName),
                new Claim(ClaimTypes.Role, "User")
            };

            // 3. Create signing key
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 4. Generate token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(_configuration["Jwt:ExpiryMinutes"])
                ),
                signingCredentials: creds
            );

            // 5. Store token in HttpOnly cookie
            Response.Cookies.Append("jwt", new JwtSecurityTokenHandler().WriteToken(token), new CookieOptions
            {
                HttpOnly = true,              // 🔒 Not accessible via JS
                Secure = true,                // 🔒 Required for HTTPS
                SameSite = SameSiteMode.None, // 🔥 Required for cross-origin (Angular ↔ API)
                Expires = DateTime.UtcNow.AddMinutes(30),
                Path = "/"
            });

            // 6. Return token
            return Ok(new UserLoginResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            });
        }

        private static bool IsValidUser(string username, string password)
        {
            // Replace with DB + hashed password check
            return username == "admin" && password == "password";
        }
    }
}
