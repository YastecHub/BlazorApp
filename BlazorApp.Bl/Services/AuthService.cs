//using BlazorApp.Bl.Interfaces.IRepository;
//using BlazorApp.Bl.Interfaces.IServices;
//using BlazorApp.Models.Entities;

//namespace BlazorApp.Bl.Services
//{
//    public class AuthService(IAuthRepository authRepository) : IAuthService
//    {
//        public async Task AddRefreshTokenModel(RefreshTokenModel refreshTokenModel)
//        {
//            await authRepository.RemoveRefreshTokenByUserID(refreshTokenModel.UserID);
//            await authRepository.AddRefreshTokenModel(refreshTokenModel);
//        }

//        public Task<RefreshTokenModel> GetRefreshTokenModel(string refreshToken)
//        {
//            return authRepository.GetRefreshTokenModel(refreshToken);
//        }

//        //public Task<UserModel> GetUserByLogin(string username, string password)
//        //{
//        //   return authRepository.GetUserByLogin(username, password);
//        //}
//    }
//}
