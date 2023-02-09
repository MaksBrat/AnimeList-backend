using AnimeList.Common.Filters.Abstract;
using AnimeList.Common.Filters.Base;
using AnimeList.Domain.Entity.Animes;
using AnimeList.Domain.Enums;
using AnimeList.Services.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AnimeList.Common.Filters
{
    public class AnimeFilter : BaseFilter<Anime>, IFilter
    {       
        public GenreFilter[]? Genres { get; set; }
        public string? AnimeType { get; set; }
        public string? AnimeStatus { get; set; }
        
        public void Filter()
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
