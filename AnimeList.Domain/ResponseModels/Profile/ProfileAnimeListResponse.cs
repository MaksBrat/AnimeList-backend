
namespace AnimeList.Domain.ResponseModels.Profile
{
    public class ProfileAnimeListResponse
    {
        public UserProfileResponse Profile { get; set; }
        public ICollection<UserAnimeListResponse> AnimeList { get; set; }
    }
}
