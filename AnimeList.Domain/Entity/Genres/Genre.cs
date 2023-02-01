using AnimeList.Domain.Entity.Animes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AnimeList.Domain.Entity.Genres
{
    public class Genre
    {
        public int Id { get; set; }
        public string GenreName { get; set; }
        public ICollection<AnimeGenre> AnimeGenre { get; set; }
    }
}
