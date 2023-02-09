using AnimeList.Domain.Entity.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace AnimeList.Domain.Entity.AnimeNews
{
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
        public string Content { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
