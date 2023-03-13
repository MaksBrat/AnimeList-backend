using AnimeList.Domain.Entity.Account;
using AnimeList.Domain.RequestModels.Account;
using AnimeList.Domain.Response;
using AnimeList.Domain.ResponseModels.Account;
using Microsoft.AspNetCore.Identity;

namespace AnimeList.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IBaseResponse<IdentityUser>> Register(RegisterRequest model);
        Task<IBaseResponse<AuthenticatedResponse>> Login(LoginRequest model);
        public Task<IBaseResponse<ApplicationUser>> Logout(string userName);
    }
}
