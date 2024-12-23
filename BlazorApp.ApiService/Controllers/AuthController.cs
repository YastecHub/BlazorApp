﻿using BlazorApp.Bl.Interfaces.IServices;
using BlazorApp.Models.Entities;
using BlazorApp.Models.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlazorApp.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController: ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;
        private readonly UserManager<User> _userManager;

        public AuthController(IConfiguration configuration, IAuthService authService, UserManager<User> userManager)
        {
            _configuration = configuration;
            _authService = authService;
            _userManager = userManager;
        }
        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponseModel>> Login([FromBody] LoginModel loginModel)
        {
            var user = await _authService.GetUserByLogin(loginModel.UserName, loginModel.Password);
            if (user != null)
            {
                var token = await GenerateJwtToken(user, isRefreshToken: false);
                var refreshToken = await GenerateJwtToken(user, isRefreshToken: true);

                await _authService.AddRefreshTokenModel(new RefreshTokenModel
                {
                    RefreshToken = refreshToken,
                    UserId = user.Id,
                });

                return Ok(new LoginResponseModel
                {
                    Token = token,
                    RefreshToken = refreshToken,
                    TokenExpired = DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds(),
                });
            }
            return Unauthorized(new
            {
                message = "Invalid username or password"
            });
        }

        [HttpGet("loging-By-RefreshToken")]
        public async Task<ActionResult<LoginResponseModel>> LoginByRefreshToken(string refreshToken)
        {
            var refreshTokenModel = await _authService.GetRefreshTokenModel(refreshToken);
            if (refreshTokenModel?.User == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            var newToken = await GenerateJwtToken(refreshTokenModel.User, isRefreshToken: false);
            var newRefreshToken = await GenerateJwtToken(refreshTokenModel.User, isRefreshToken: true);

            await _authService.AddRefreshTokenModel(new RefreshTokenModel
            {
                RefreshToken = newRefreshToken,
                UserId = refreshTokenModel.UserId,
            });
            return new LoginResponseModel
            {
                Token = newToken,
                RefreshToken = newRefreshToken,
                TokenExpired = DateTimeOffset.Now.AddHours(1).ToUnixTimeSeconds()
            };
        }

        private async Task<string> GenerateJwtToken(User user, bool isRefreshToken)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            var userRoles = await _userManager.GetRolesAsync(user);

            if (userRoles.Any())
            {
                claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));
            }

            string secret = _configuration.GetValue<string>($"Jwt:{(isRefreshToken ? "RefreshTokenSecret" : "Secret")}");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "yasirola",
                audience: "yasirola",
                claims: claims,
                expires: DateTime.Now.AddHours(isRefreshToken ? 24 : 1),
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
