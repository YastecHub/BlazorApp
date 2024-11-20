using BlazorApp.Bl.Interfaces.IRepository;
using BlazorApp.Bl.Interfaces.IServices;
using BlazorApp.Models.Entities;

namespace BlazorApp.Bl.Services
{
    public class AuthService(IAuthRepository authRepository) : IAuthService
    {
        public async Task AddRefreshTokenModel(RefreshTokenModel refreshTokenModel)
        {
            await authRepository.RemoveRefreshTokenByUserID(refreshTokenModel.UserId);
            await authRepository.AddRefreshTokenModel(refreshTokenModel);
        }

        public Task<RefreshTokenModel> GetRefreshTokenModel(string refreshToken)
        {
            return authRepository.GetRefreshTokenModel(refreshToken);
        }

        public Task<User> GetUserByLogin(string username, string password)
        {
            return authRepository.GetUserByLogin(username, password);
        }

        public async Task<List<string>> GetUserRoles(string userId)
        {
           return await authRepository.GetUserRoles(userId);
        }
    }
}
