using AnimeList.Domain.Entity.Account;
using AnimeList.Domain.Entity.AnimeNews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeList.Domain.ResponseModels.AnimeNews
{
    public class NewsResponseModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int AuthorId { get; set; }
        public byte[] AuthorAvatar { get; set; }
        public string Content { get; set; }
        public string DateCreated { get; set; }
        public ICollection<CommentResponseModel> Comments { get; set; }
    }
}
