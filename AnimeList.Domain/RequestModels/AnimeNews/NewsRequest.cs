using AnimeList.Domain.Entity.Account;
using AnimeList.Domain.Entity.AnimeNews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeList.Domain.RequestModels.AnimeNews
{
    public class NewsRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
