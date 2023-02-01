using AnimeList.Domain.Entity.Account;
using AnimeList.Domain.RequestModels;
using AnimeList.Domain.Response;
using AnimeList.Domain.ResponseModels.Profile;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace AnimeList.Services.Interfaces
{
    public interface IProfileService
    {
        public IBaseResponse<UserProfileResponseModel> Edit(ProfileRequestModel model, int userId);
        public IBaseResponse<UserProfileResponseModel> ChangeAvatar(IFormFile avatar, int userId);
        public Task<IBaseResponse<UserProfile>> Create(ApplicationUser user);
        public Task<IBaseResponse<UserProfileResponseModel>> Get(int UserId);
        public IBaseResponse<ProfileAnimeListResponseModel> GetProfileWithAnimeList(int userId);
        public IBaseResponse<bool> AddAnimeToList(int userId, int animeId);
        public IBaseResponse<bool> DeleteAnimeFromList(int animeId, int userId);
        public IBaseResponse<bool> ChangeUserRating(int id, int rating);
        public IBaseResponse<bool> ChangeAnimeStatus(int id, string status);
        public IBaseResponse<bool> ChangeWatchedEpisodes(int id, int episodes);
    }
}
