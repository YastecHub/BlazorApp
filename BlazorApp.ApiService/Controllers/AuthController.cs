using BlazorApp.Bl.Interfaces.IServices;
using BlazorApp.Models.Entities;
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
    public class AuthController(IConfiguration configuration, IAuthService authService) : ControllerBase
    {
        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponseModel>> Login([FromBody] LoginModel loginModel)
        {
            var user = await authService.GetUserByLogin(loginModel.UserName, loginModel.Password);
            if (user != null)
            {
                var token = GenerateJwtToken(user, isRefreshToken: false);
                var refreshToken = GenerateJwtToken(user, isRefreshToken: true);

                await authService.AddRefreshTokenModel(new RefreshTokenModel
                {
                    RefreshToken = refreshToken,
                    UserID = user.ID
                });

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
        public async Task <ActionResult<LoginResponseModel>> LoginByRefreshToken(string refreshToken)
        {
            var refreshTokenModel = await authService.GetRefreshTokenModel(refreshToken);
            if (refreshTokenModel == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            var newToken = GenerateJwtToken(refreshTokenModel.User, isRefreshToken: false);
            var newRefreshToken = GenerateJwtToken(refreshTokenModel.User, isRefreshToken: true);

            await authService.AddRefreshTokenModel(new RefreshTokenModel
            {
                RefreshToken =newRefreshToken,
                UserID = refreshTokenModel.UserID,
            });
            return new LoginResponseModel
            {
                Token = newToken,
                TokenExpired = DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds(),
                RefreshToken = newRefreshToken
            };
        }

        private string GenerateJwtToken(UserModel user, bool isRefreshToken)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
            };
            claims.AddRange(user.UserRoles.Select(n => new Claim(ClaimTypes.Role, n.Role.RoleName)));
            string secret = configuration.GetValue<string>($"Jwt:{(isRefreshToken ? "RefreshTokenSecret" : "Secret")}");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "yasirola",
                audience: "yasirola",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(isRefreshToken ? 24 : 1),
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
