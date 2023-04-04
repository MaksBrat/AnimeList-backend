using AnimeList.Common.EntitiesFilters.Abstract;
using AnimeList.Domain.Entity.Animes;
using AnimeList.Domain.Enums;
using AnimeList.Domain.RequestModels.EntitiesFilters;
using AnimeList.Services.Extentions;
using System.Linq.Expressions;

namespace AnimeList.Common.EntitiesFilters
{
    public class AnimeFilter : BaseFilter<Anime>
    {       
        public AnimeGenreFilterRequest[]? Genres { get; set; }
        public string? AnimeType { get; set; }
        public string? AnimeStatus { get; set; }
        
        public override void CreateFilter()
        {
            ApplySearchQueryFilter(nameof(Anime.Title));
            ApplyEnumFilter<AnimeType>(AnimeType, nameof(Anime.AnimeType));
            ApplyEnumFilter<AnimeStatus>(AnimeStatus, nameof(Anime.AnimeStatus));

            if(Genres != null)
            {
                ApplyGenreFilter();
            }
            
            ApplyOrderByFilter(OrderBy, AscOrDesc);         
        }

        private void ApplyGenreFilter()
        {
            var genres = Genres.Where(x => x.Checked == true).Select(x => x.Name).ToList();
            if (genres.Count != 0)
            {
                Expression<Func<Anime, bool>> predicateGenres = a => a.AnimeGenres.Where(ag => genres.Contains(ag.Genre.Name)).Count() == genres.Count();
                Predicate = Predicate == null ? predicateGenres : Predicate.And(predicateGenres);
            }
        }
    }
}
