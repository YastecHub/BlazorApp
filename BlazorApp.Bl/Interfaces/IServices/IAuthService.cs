using BlazorApp.Models.Entities;

namespace BlazorApp.Bl.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<User> GetUserByLogin(string username, string password);
        Task AddRefreshTokenModel(RefreshTokenModel refreshTokenModel);
        Task<RefreshTokenModel> GetRefreshTokenModel(string refreshToken);
        Task<List<string>> GetUserRoles(string userId);
    }
}
