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
                var token = GenerateJwtToken(loginModel.UserName, isRefreshToken: false);
                var refreshToken = GenerateJwtToken(loginModel.UserName, isRefreshToken: true);
                return Ok(new LoginResponseModel
                {
                    Token = token,
                    RefreshToken = refreshToken,
                    TokenExpired = DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds(),
                });
            }
            return null;
        }

        [HttpGet("logingByRefreshToken")]
        public ActionResult<LoginResponseModel> LoginByRefreshToken(string refreshToken)
        {
            var secret = configuration.GetValue<string>("jwt:RefreshTokenSecret");
            var claimsPrincipal = GetClaimsPrincipalFromToken(refreshToken, secret);
            if(claimsPrincipal != null)
            {
                return new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }
            var userName = claimsPrincipal.FindFirstValue(ClaimTypes.Name);
            //Call to Db to check user valid

            var newToken = GenerateJwtToken(userName, isRefreshToken: false);
            var newRefreshToken = GenerateJwtToken(userName, isRefreshToken:true);
            return new LoginResponseModel
            {
                Token = newToken,
                TokenExpired = DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds(),
                RefreshToken = newRefreshToken
            };

        }

        private ClaimsPrincipal GetClaimsPrincipalFromToken(string token, string secret)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidAudience = "yastec",
                    ValidateIssuer = true,
                    ValidIssuer = "yastec",
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                }, out var validatedToken);
                return principal;
            }
            catch
            {
                return null;
            }
        }

        private string GenerateJwtToken(string userName, bool isRefreshToken)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, userName),
            new Claim(ClaimTypes.Role, userName == "Admin" ? "Admin" : "User")
        };
            string secret = configuration.GetValue<string>($"Jwt:{(isRefreshToken ? "RefreshTokenSecret" : "Secret")}");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer:  "yasirola",
                audience: "yasirola",
                claims : claims,
                expires: DateTime.UtcNow.AddHours(isRefreshToken ? 24 : 1),
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
