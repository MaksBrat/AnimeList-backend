﻿using AnimeList.Domain.Entity.Account;
using AnimeList.Domain.Entity.Animes;
using AnimeList.Domain.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeList.Domain.ResponseModels.Profile
{
    public class UserAnimeListResponse
    {
        public int Id { get; set; }
        public float UserRating { get; set; }
        public string AnimeStatus { get; set; }
        public int WatchedEpisodes { get; set; }
        public AnimeResponse Anime { get; set; }
    }
}
