using AnimeList.Domain.Entity.Account;
using AnimeList.Domain.Entity.AnimeNews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeList.Domain.ResponseModels.AnimeNews
{
    public class CommentResponseModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public int AuthorId { get; set; }
        public byte[] AuthorAvatar { get; set; }
        public int NewsId { get; set; }
        public string DateCreated { get; set; }
    }
}
