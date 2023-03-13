using AnimeList.Domain.Entity.Account;
using AnimeList.Domain.RequestModels;
using AnimeList.Domain.Response;
using AnimeList.Domain.ResponseModels.Profile;
using Microsoft.AspNetCore.Http;

namespace AnimeList.Services.Interfaces
{
    public interface IProfileService
    {
        public IBaseResponse<UserProfileResponse> Edit(ProfileRequest model, int userId);
        public Task<IBaseResponse<UserProfileResponse>> ChangeAvatar(IFormFile avatar, int userId);
        public Task<IBaseResponse<UserProfileResponse>> Create(ApplicationUser user);
        public Task<IBaseResponse<UserProfileResponse>> Get(int UserId);
        public IBaseResponse<ProfileAnimeListResponse> GetProfileWithAnimeList(int userId);
        public IBaseResponse<bool> AddAnimeToList(int userId, int animeId);
        public IBaseResponse<bool> DeleteAnimeFromList(int animeId, int userId);
        public IBaseResponse<bool> ChangeUserRating(int id, int? rating);
        public IBaseResponse<bool> ChangeAnimeStatus(int id, string status);
        public IBaseResponse<bool> ChangeWatchedEpisodes(int id, int? episodes);
    }
}
