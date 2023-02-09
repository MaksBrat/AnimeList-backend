using AnimeList.Common.Filters.Abstract;
using AnimeList.Common.Filters.Base;
using AnimeList.Domain.Entity.AnimeNews;
using AnimeList.Domain.Entity.Animes;
using AnimeList.Domain.Entity.Genres;
using AnimeList.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeList.Common.Filters
{
    public class NewsFilter : BaseFilter<News>, IFilter 
    {
        public void Filter()
        {
            base.ApplySearchQueryFilter(nameof(News.Title));
        
            base.ApplyOrderByFilter(OrderBy, AscOrDesc);
        }

    }
}
