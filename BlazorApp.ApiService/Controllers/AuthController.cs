using BlazorApp.Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlazorApp.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IConfiguration configuration) : ControllerBase
    {
        [HttpPost("Login")]
        public ActionResult<LoginResponseModel> Login([FromBody] LoginModel loginModel)
        {
            if ((loginModel.UserName == "Admin" && loginModel.Password == "Admin") || (loginModel.UserName == "User" && loginModel.Password == "User"))
            {
                var token = GenerateJwtToken(loginModel.UserName);
                return Ok(new LoginResponseModel
                {
                    Token = token,
                });
            }
            return null;
        }

        private string GenerateJwtToken(string userName)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, userName),
            new Claim(ClaimTypes.Role, userName == "Admin" ? "Admin" : "User")
        };
            string secret = configuration.GetValue<string>("Jwt:Secret");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer:  "yasirola",
                audience: "yasirola",
                claims : claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
