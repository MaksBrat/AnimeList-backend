using AnimeList.Common.EntitiesFilters.Abstract;
using AnimeList.Common.EntitiesFilters.Base;
using AnimeList.Domain.Entity.Animes;
using AnimeList.Domain.Enums;
using AnimeList.Domain.RequestModels.EntitiesFilters;
using AnimeList.Services.Extentions;
using System.Linq.Expressions;

namespace AnimeList.Common.EntitiesFilters
{
    public class AnimeFilter : BaseFilter<Anime>, IFilter
    {       
        public AnimeGenreFilterRequest[]? Genres { get; set; }
        public string? AnimeType { get; set; }
        public string? AnimeStatus { get; set; }
        
        public void CreateFilter()
        {
            base.ApplySearchQueryFilter(nameof(Anime.Title));
            base.ApplyEnumFilter<AnimeType>(AnimeType, nameof(Anime.AnimeType));
            base.ApplyEnumFilter<AnimeStatus>(AnimeStatus, nameof(Anime.AnimeStatus));

            if(Genres != null)
            {
                ApplyGenreFilter();
            }
            
            base.ApplyOrderByFilter(OrderBy, AscOrDesc);         
        }

        private void ApplyGenreFilter()
        {
            var genres = Genres.Where(x => x.Checked == true).Select(x => x.Name).ToList();
            if (genres.Count != 0)
            {
                Expression<Func<Anime, bool>> predicateGenres = a => a.AnimeGenres.Where(ag => genres.Contains(ag.Genre.Name)).Count() == genres.Count();
                base.Predicate = base.Predicate == null ? predicateGenres : base.Predicate.And(predicateGenres);
            }
        }
    }
}
