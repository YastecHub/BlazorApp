using BlazorApp.Bl.Interfaces.IRepository;
using BlazorApp.Database.Data;
using BlazorApp.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Bl.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _dbContext;
        private readonly RoleManager<User> _roleManager;

        public AuthRepository(UserManager<User> userManager, AppDbContext dbContext, RoleManager<User> roleManager)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _roleManager = roleManager;
        }
        public async Task AddRefreshTokenModel(RefreshTokenModel refreshTokenModel)
        {
            await _dbContext.RefreshTokens.AddAsync(refreshTokenModel);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<RefreshTokenModel> GetRefreshTokenModel(string refreshToken)
        {
            var refreshTokenModel = await _dbContext.RefreshTokens.
                Include(n => n.User).
                FirstOrDefaultAsync(n => n.RefreshToken == refreshToken);

            if (refreshTokenModel?.User != null)
            {
                var userRoles = await _userManager.GetRolesAsync(refreshTokenModel.User);

                refreshTokenModel.User.Roles = userRoles.ToList();
            }
            return refreshTokenModel;
        }

        public async Task<User> GetUserByLogin(string username, string password)
        {
           var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
                if (isPasswordValid)
                {
                    return user;
                }
            }
            return null;
        }

        public async Task<List<string>> GetUserRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                return await _userManager.GetRolesAsync(user) as List<string>;
            }
            return new List<string>();
        }

        public async Task RemoveRefreshTokenByUserID(string userID)
        {
            var refreshToken = _dbContext.RefreshTokens.FirstOrDefault(n => n.UserId == userID);
            if (refreshToken != null)
            {
                _dbContext.RemoveRange(refreshToken);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
