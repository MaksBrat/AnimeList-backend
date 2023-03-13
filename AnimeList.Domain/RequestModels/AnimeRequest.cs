using System.ComponentModel.DataAnnotations;

namespace AnimeList.Domain.RequestModels
{
    public class AnimeRequest
    {
        public int Id { get; set; }

        [Range(1, 10, ErrorMessage = "Invalid rating")]
        public float Rating { get; set; }
        public string? Title { get; set; }

        [MinLength(1)]
        public int Episodes { get; set; }

        [MinLength(1)]
        public int EpisodeDuration { get; set; }
        public string? AnimeType { get; set; }
        public string AnimeStatus { get; set; }
        public string? ReleaseDate { get; set; }
        public string? PosterUrl { get; set; }
        public string? TrailerUrl { get; set; }
        public ICollection<GenreRequest> Genres { get; set; }
    }
}
