using AnimeList.Domain.Entity.Genres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnimeList.Domain.Entity.Animes
{
    public class AnimeGenre
    {
        public int AnimeId { get; set; }
        public Anime Anime { get; set; }
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
