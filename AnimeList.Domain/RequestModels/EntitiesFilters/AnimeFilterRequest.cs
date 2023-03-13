using AnimeList.Domain.RequestModels.EntitiesFilters.Base;

namespace AnimeList.Domain.RequestModels.EntitiesFilters
{
    public class AnimeFilterRequest : BaseFilterRequest
    {
        public AnimeGenreFilterRequest[]? Genres { get; set; }
        public string? AnimeType { get; set; }
        public string? AnimeStatus { get; set; }
    }
}
