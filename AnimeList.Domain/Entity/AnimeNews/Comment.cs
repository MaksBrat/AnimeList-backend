using AnimeList.Domain.Entity.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeList.Domain.Entity.AnimeNews
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
        public int NewsId { get; set; }
        public News News { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
