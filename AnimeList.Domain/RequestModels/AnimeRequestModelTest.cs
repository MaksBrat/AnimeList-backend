using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeList.Domain.RequestModels
{
    public class AnimeRequestModelTest
    {
        public int? id { get; set; }
        public string? Title { get; set; }
        public float? Rating { get; set; } 
        public int? Episodes { get; set; }
        public int? EpisodeDuration { get; set; }
        public string? AnimeType { get; set; }
        public string? RealizeDate { get; set; }
        public ICollection<GenreRequestModel> Genres { get; set; }
    }
}
