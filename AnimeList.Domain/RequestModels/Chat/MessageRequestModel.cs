using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeList.Domain.RequestModels.Chat
{
    public class MessageRequestModel
    {
        public int AuthorId { get; set; }
        public string Text { get; set; }

    }
}
