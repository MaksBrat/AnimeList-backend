using AnimeList.Common.Constants;
using AnimeList.Common.EntitiesFilters;
using AnimeList.Common.Utility;
using AnimeList.Domain.Chat;
using AnimeList.Domain.Entity.Account;
using AnimeList.Domain.Entity.AnimeNews;
using AnimeList.Domain.Entity.Animes;
using AnimeList.Domain.RequestModels;
using AnimeList.Domain.RequestModels.AnimeNews;
using AnimeList.Domain.RequestModels.Chat;
using AnimeList.Domain.RequestModels.EntitiesFilters;
using AnimeList.Domain.ResponseModel;
using AnimeList.Domain.ResponseModels.AnimeNews;
using AnimeList.Domain.ResponseModels.Chat;
using AnimeList.Domain.ResponseModels.Profile;
using System.Globalization;

namespace AnimeList.Services
{
    public class AppMappingProfile : AutoMapper.Profile
    {
        public AppMappingProfile()
        {

            #region Filters

            CreateMap<AnimeFilterRequest, AnimeFilter>();
            CreateMap<NewsFilterRequest, NewsFilter>();
            CreateMap<CommentFilterRequest, CommentFilter>();

            #endregion

            #region Anime
            CreateMap<Anime, AnimeResponse>()
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.AnimeGenres
                    .Select(x => new GenreResponse
                    {
                        Id = x.GenreId,
                        Name = x.Genre.Name
                    }).ToList()))
                .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src => src.ReleaseDate.ToString("yyyy-MM-dd")));

            CreateMap<AnimeRequest, Anime>()
                .ForMember(dest => dest.TrailerUrl, opt => opt.MapFrom(src => src.TrailerUrl != null ? UrlParser.ParseTrailerUrl(src.TrailerUrl) : null))
                .ForMember(dest => dest.PosterUrl, opt => opt.MapFrom(src => src.PosterUrl != null ? src.PosterUrl : AnimeConstants.POSTER_URL))
                .ForMember(dest => dest.ReleaseDate, opt =>
                    opt.MapFrom(src => DateTime.ParseExact(src.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                ));

            #endregion

            #region Profile

            CreateMap<ProfileRequest, UserProfile>();

            CreateMap<UserProfile, UserProfileResponse>()
                .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.FileModel.Path));

            CreateMap<UserAnimeList, UserAnimeListResponse>();

            #endregion

            #region News

            CreateMap<News, NewsResponse>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author.Profile.Name))
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
                .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.Author.Profile.FileModel.Path))
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated.ToString("yyyy-MM-dd")));

            CreateMap<NewsRequest, News>()
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => DateTime.Now));

            #endregion

            #region Comment

            CreateMap<Comment, CommentResponse>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author.Profile.Name))
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
                .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.Author.Profile.FileModel.Path))
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated.ToString("yyyy-MM-dd")));

            CreateMap<CommentRequest, Comment>()
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => DateTime.Now));


            #endregion

            #region Message

            CreateMap<Message, MessageResponse>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author.Profile.Name))
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
                .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.Author.Profile.FileModel.Path))
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated.ToString("yyyy-MM-dd"))); ;

            CreateMap<MessageRequest,  Message>()
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => DateTime.Now));

            #endregion
        }
    }
}
