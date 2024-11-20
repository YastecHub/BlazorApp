using BlazorApp.Models.Entities;

namespace BlazorApp.Bl.Interfaces.IRepository
{
    public interface IAuthRepository
    {
        Task<User> GetUserByLogin(string username, string password);
        Task RemoveRefreshTokenByUserID(string userID);
        Task AddRefreshTokenModel(RefreshTokenModel refreshTokenModel);
        Task<RefreshTokenModel> GetRefreshTokenModel(string refreshToken);
        Task<List<string>> GetUserRoles(string userId);
    }
}
