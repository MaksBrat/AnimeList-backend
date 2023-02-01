using AnimeList.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnimeList.Domain.Entity.Animes
{
    public class Anime
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public float Rating { get; set; } // based on SelfRating each user
        public int Episodes { get; set; }
        public int EpisodeDuration { get; set; }
        public AnimeType AnimeType { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string? PosterUrl { get; set; } 
        public string? TrailerUrl { get; set; } 
        public ICollection<AnimeGenre> AnimeGenres { get; set; }
    }  
}
