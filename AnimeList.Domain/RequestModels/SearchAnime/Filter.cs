using AnimeList.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeList.Domain.RequestModels.SearchAnime
{
    public class Filter
    {
        public string? SearchQuery { get; set; }
        public GenreFilter[]? Genres { get; set; }
        public string? AnimeType { get; set; }
        public string? AnimeStatus { get; set; }
        public string? OrderBy { get; set; }
        public string? AscOrDesc { get; set; }
        public int Take { get; set; } = 0;
    }
}
