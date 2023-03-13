using AnimeList.Common.EntitiesFilters.Abstract;
using AnimeList.Common.EntitiesFilters.Base;
using AnimeList.Domain.Entity.AnimeNews;

namespace AnimeList.Common.EntitiesFilters
{
    public class NewsFilter : BaseFilter<News>, IFilter 
    {
        public void CreateFilter()
        {
            base.ApplySearchQueryFilter(nameof(News.Title));
        
            base.ApplyOrderByFilter(OrderBy, AscOrDesc);
        }

    }
}
