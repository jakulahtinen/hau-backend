using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using hau_backend.Models;

namespace hau_backend.Controllers
{
    [Route("api/auth[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly JwtSettings _jwtSettings;

        public AuthController(IConfiguration configuration, IOptions<JwtSettings> jwtSettings)
        {
            _configuration = configuration;
            _jwtSettings = jwtSettings.Value;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {

            var adminUsername = _configuration["AdminCredentials:Username"];
            var adminPassword = _configuration["AdminCredentials:Password"];

            if (request.Username == adminUsername && request.Password == adminPassword)
            {
                // JWT-token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, request.Username),
                        new Claim(ClaimTypes.Role, "Admin") 
                    }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    Issuer = _jwtSettings.Issuer, // 
                    Audience = _jwtSettings.Audience,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return Ok(new { token = tokenHandler.WriteToken(token) });
            }

            return Unauthorized("Käyttäjänimi tai salasana on virheellinen");
        }
    }
}

public class LoginRequest
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}