using AnimeList.Common.EntitiesFilters.Abstract;
using AnimeList.Common.EntitiesFilters.Base;
using AnimeList.Domain.Entity.Animes;

namespace AnimeList.Common.EntitiesFilters
{
    public class CommentFilter : BaseFilter<Anime>, IFilter
    {
        public void CreateFilter()
        {
            base.ApplyOrderByFilter(OrderBy, AscOrDesc);
        }
    }
}
