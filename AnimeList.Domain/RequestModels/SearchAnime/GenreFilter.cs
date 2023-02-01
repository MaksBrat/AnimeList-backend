using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeList.Domain.RequestModels.SearchAnime
{
    public class GenreFilter
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public bool? Checked { get; set; }
    }
}
