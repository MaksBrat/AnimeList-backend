using AnimeList.Domain.Entity.Account;
using AnimeList.Domain.RequestModels.Account;
using AnimeList.Domain.Response;
using AnimeList.Domain.ResponseModels.Account;
using Microsoft.AspNetCore.Identity;

namespace AnimeList.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IBaseResponse<IdentityUser>> Register(RegisterModel model);
        Task<IBaseResponse<AuthenticatedResponse>> Login(LoginModel model);
        public Task<IBaseResponse<ApplicationUser>> Logout(string userName);
    }
}
