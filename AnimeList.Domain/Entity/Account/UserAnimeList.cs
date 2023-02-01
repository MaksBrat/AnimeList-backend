using AnimeList.Domain.Entity.Animes;
using AnimeList.Domain.Enum;

namespace AnimeList.Domain.Entity.Account
{
    public class UserAnimeList
    {    
        public int Id { get; set; }
        public float UserRating { get; set; }
        public int WatchedEpisodes { get; set; }
        public AnimeStatus AnimeStatus { get; set; }
        public int AnimeId { get; set; }
        public Anime Anime { get; set; }
        public int ProfileId { get; set; }
        public UserProfile Profile { get; set; }
    }
}
