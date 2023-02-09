using AnimeList.Domain.Entity.Animes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeList.Domain.RequestModels
{
    public class AnimeRequestModel
    {
        public int Id { get; set; } 
        public string? Title { get; set; }
        public int Episodes { get; set; }
        public int EpisodeDuration { get; set; }
        public string? AnimeType { get; set; }
        public string AnimeStatus { get; set; }
        public string? ReleaseDate { get; set; }
        public string? PosterUrl { get; set; }
        public string? TrailerUrl { get; set; }
        public ICollection<GenreRequestModel> Genres { get; set; }
    }
}
