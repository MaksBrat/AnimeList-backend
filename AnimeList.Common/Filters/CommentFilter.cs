using AnimeList.Common.Filters.Abstract;
using AnimeList.Common.Filters.Base;
using AnimeList.Domain.Entity.Animes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeList.Common.Filters
{
    public class CommentFilter : BaseFilter<Anime>, IFilter
    {
        public void Filter()
        {
            base.ApplyOrderByFilter(OrderBy, AscOrDesc);
        }
    }
}
