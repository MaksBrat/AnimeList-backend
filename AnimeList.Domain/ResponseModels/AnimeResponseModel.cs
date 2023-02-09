using AnimeList.Domain.Entity.Animes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeList.Domain.ResponseModel
{
    public class AnimeResponseModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public float Rating { get; set; } // based on SelfRating each user
        public int? Episodes { get; set; }
        public int? EpisodeDuration { get; set; }
        public string AnimeType { get; set; }
        public string AnimeStatus { get; set; }
        public string ReleaseDate { get; set; }  
        public string? PosterUrl { get; set; }
        public string? TrailerUrl { get; set; }
        public ICollection<GenreResponseModel> Genres { get; set; }
    }
}
