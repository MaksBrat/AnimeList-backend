using AnimeList.Domain.Entity.Account;
using AnimeList.Domain.RequestModels.Account;
using AnimeList.Domain.Response;
using AnimeList.Domain.ResponseModels.Account;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeList.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IBaseResponse<IdentityUser>> Register(RegisterModel model);
        Task<IBaseResponse<AuthenticatedResponse>> Login(LoginModel model);
        public Task<IBaseResponse<ApplicationUser>> Logout(string userName);
    }
}
