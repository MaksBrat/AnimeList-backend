using AnimeList.Common.Constants;
using AnimeList.Common.Utitlities;
using AnimeList.Domain.Entity.Account;
using AnimeList.Domain.Entity.Animes;
using AnimeList.Domain.Enum;
using AnimeList.Domain.RequestModels;
using AnimeList.Domain.ResponseModel;
using AnimeList.Domain.ResponseModels.Profile;
using AnimeList.Services.Extentions;
using System.Globalization;

namespace AnimeList.Services
{
    public class AppMappingProfile : AutoMapper.Profile
    {
        public AppMappingProfile()
        {
            #region Anime
            CreateMap<Anime, AnimeResponseModel>()
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.AnimeGenres
                    .Select(x => new GenreResponseModel
                    {
                        Id = x.GenreId,
                        Name = x.Genre.Name
                    }).ToList()))
                .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src => src.ReleaseDate.ToString("yyyy-MM-dd")));

            CreateMap<AnimeRequestModel, Anime>()
                .ForMember(dest => dest.TrailerUrl, opt => opt.MapFrom(src => src.TrailerUrl != null ? UrlParser.ParseTrailerUrl(src.TrailerUrl) : null))
                .ForMember(dest => dest.PosterUrl, opt => opt.MapFrom(src => src.PosterUrl != null ? src.PosterUrl : AnimeConstans.POSTER_URL))
                .ForMember(dest => dest.ReleaseDate, opt =>
                    opt.MapFrom(src => DateTime.ParseExact(src.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                ));

            #endregion

            #region Profile
            CreateMap<ProfileRequestModel, UserProfile>();

            CreateMap<UserProfile, UserProfileResponseModel>();
            CreateMap<UserAnimeList, UserAnimeListResponseModel>()
                .ForMember(dest => dest.AnimeStatus, opt => opt.MapFrom(src => src.AnimeStatus.ToString()));

            #endregion


        }
    }
}
