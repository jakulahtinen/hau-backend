using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace hau_backend.Controllers
{
    [Route("api/auth[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Simppeli käyttäjätarkistus (korvaa oikealla tietokantatarkistuksella)
            if (request.Username == "admin" && request.Password == "password123")
            {
                // Luo JWT-token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes("your_secret_key"); // Varmista, että tämä on vahva avain
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                    new Claim(ClaimTypes.Name, request.Username),
                    new Claim(ClaimTypes.Role, "Admin") // Käyttäjärooli
                }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    Issuer = "yourapp",
                    Audience = "yourapp",
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