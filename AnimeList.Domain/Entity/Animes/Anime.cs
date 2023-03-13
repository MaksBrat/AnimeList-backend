using AnimeList.Domain.Enums;

namespace AnimeList.Domain.Entity.Animes
{
    public class Anime
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public float Rating { get; set; } 
        public int Episodes { get; set; }
        public int EpisodeDuration { get; set; }
        public AnimeType AnimeType { get; set; }
        public AnimeStatus AnimeStatus { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string? PosterUrl { get; set; } 
        public string? TrailerUrl { get; set; } 
        public ICollection<AnimeGenre> AnimeGenres { get; set; }
    }  
}
