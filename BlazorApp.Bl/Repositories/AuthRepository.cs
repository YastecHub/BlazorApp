//using BlazorApp.Bl.Interfaces.IRepository;
//using BlazorApp.Database.Data;
//using BlazorApp.Models.Entities;
//using Microsoft.EntityFrameworkCore;

//namespace BlazorApp.Bl.Repositories
//{
//    public class AuthRepository(AppDbContext dbContext) : IAuthRepository
//    {
//        public async Task AddRefreshTokenModel(RefreshTokenModel refreshTokenModel)
//        {
//            await dbContext.RefreshTokens.AddAsync(refreshTokenModel);
//            await dbContext.SaveChangesAsync();
//        }

//        public Task<RefreshTokenModel> GetRefreshTokenModel(string refreshToken)
//        {
//            return dbContext.RefreshTokens.
//                Include(n => n.User).
//                ThenInclude(n => n.UserRoles).
//                ThenInclude(n => n.Role).
//                FirstOrDefaultAsync(n => n.RefreshToken == refreshToken);
//        }

//        //public Task<UserModel> GetUserByLogin(string username, string password)
//        //{
//        //   return dbContext.Users.
//        //        Include(n => n.UserRoles).
//        //        ThenInclude(n => n.Role).
//        //        FirstOrDefaultAsync(n => n.UserName == username && n.PassWord == password);
//        //}

//        public async Task RemoveRefreshTokenByUserID(int userID)
//        {
//          var refreshToken = dbContext.RefreshTokens.FirstOrDefault(n => n.UserID == userID);
//            if (refreshToken != null)
//            {
//                dbContext.RemoveRange(refreshToken);
//                await dbContext.SaveChangesAsync();
//            }
//        }
//    }
//}
