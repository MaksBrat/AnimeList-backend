using AnimeList.Domain.Entity.Account;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AnimeList.Domain.Chat
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
