﻿using AnimeList.Domain.RequestModels.EntitiesFilters.Base;

namespace AnimeList.Domain.RequestModels.EntitiesFilters
{
    public class AnimeGenreFilterRequest
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public bool? Checked { get; set; }
    }
}
