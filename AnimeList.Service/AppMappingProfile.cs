using AnimeList.Common.Constants;
using AnimeList.Common.Utitlities;
using AnimeList.Domain.Chat;
using AnimeList.Domain.Entity.Account;
using AnimeList.Domain.Entity.AnimeNews;
using AnimeList.Domain.Entity.Animes;
using AnimeList.Domain.RequestModels;
using AnimeList.Domain.RequestModels.AnimeNews;
using AnimeList.Domain.RequestModels.Chat;
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
            CreateMap<UserAnimeList, UserAnimeListResponseModel>();
             

            #endregion

            #region News

            CreateMap<News, NewsResponseModel>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author.Profile.Name))
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.Author.Id))
                .ForMember(dest => dest.AuthorAvatar, opt => opt.MapFrom(src => src.Author.Profile.Avatar))
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated.ToString("yyyy-MM-dd")));

            CreateMap<NewsRequestModel, News>()
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => DateTime.Now));

            #endregion

            #region Comment

            CreateMap<Comment, CommentResponseModel>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author.Profile.Name))
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.Author.Id))
                .ForMember(dest => dest.AuthorAvatar, opt => opt.MapFrom(src => src.Author.Profile.Avatar))
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated.ToString("yyyy-MM-dd")));

            CreateMap<CommentRequestModel, Comment>()
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => DateTime.Now));


            #endregion

            #region Message

            CreateMap<Message, MessageResponseModel>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author.Profile.Name))
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.Author.Id))
                .ForMember(dest => dest.AuthorAvatar, opt => opt.MapFrom(src => src.Author.Profile.Avatar))
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated.ToString("yyyy-MM-dd"))); ;

            CreateMap<MessageRequestModel,  Message>()
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => DateTime.Now));

            #endregion
        }
    }
}
