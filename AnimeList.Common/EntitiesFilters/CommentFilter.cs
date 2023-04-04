using AnimeList.Common.EntitiesFilters.Abstract;
using AnimeList.Domain.Entity.Animes;

namespace AnimeList.Common.EntitiesFilters
{
    public class CommentFilter : BaseFilter<Anime>
    {
        public override void CreateFilter()
        {
            ApplyOrderByFilter(OrderBy, AscOrDesc);
        }
    }
}
