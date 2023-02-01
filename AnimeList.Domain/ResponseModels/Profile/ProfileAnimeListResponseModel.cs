
namespace AnimeList.Domain.ResponseModels.Profile
{
    public class ProfileAnimeListResponseModel
    {
        public UserProfileResponseModel Profile { get; set; }
        public ICollection<UserAnimeListResponseModel> AnimeList { get; set; }
    }
}
