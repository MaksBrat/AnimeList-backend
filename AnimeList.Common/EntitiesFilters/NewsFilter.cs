using AnimeList.Common.EntitiesFilters.Abstract;
using AnimeList.Domain.Entity.AnimeNews;

namespace AnimeList.Common.EntitiesFilters
{
    public class NewsFilter : BaseFilter<News> 
    {
        public override void CreateFilter()
        {
            ApplySearchQueryFilter(nameof(News.Title));
        
            ApplyOrderByFilter(OrderBy, AscOrDesc);
        }

    }
}
