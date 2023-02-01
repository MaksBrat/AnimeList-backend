using AnimeList.Common.Constants;
using AnimeList.Common.Extentions;
using AnimeList.Common.Utitlities;
using AnimeList.DAL.Interfaces;
using AnimeList.Domain.Entity.Animes;
using AnimeList.Domain.Entity.Genres;
using AnimeList.Domain.Enum;
using AnimeList.Domain.RequestModels;
using AnimeList.Domain.RequestModels.SearchAnime;
using AnimeList.Domain.Response;
using AnimeList.Domain.ResponseModel;
using AnimeList.Services.Extentions;
using AnimeList.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq.Expressions;
using System.Net;

namespace AnimeList.Services.Services
{
    public class AnimeService : IAnimeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AnimeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IBaseResponse<bool> Delete(int id)
        {
            try
            {
                _unitOfWork.GetRepository<Anime>().Delete(id);
                _unitOfWork.SaveChanges();

                return new BaseResponse<bool>
                {
                    Data = true,
                    StatusCode = HttpStatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>
                {
                    Description = $"Error Delete: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public IBaseResponse<AnimeResponseModel> Edit(AnimeRequestModel model)
        {
            try
            {
                var anime = _unitOfWork.GetRepository<Anime>().GetFirstOrDefault(
                    predicate: x => x.Id == model.Id,
                    include: i => i
                            .Include(x => x.AnimeGenres)
                                .ThenInclude(x => x.Genre));
                if (anime == null)
                {
                    return new BaseResponse<AnimeResponseModel>
                    {
                        Description = "Anime not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }              

                if (model.PosterUrl != null)
                {
                    anime.PosterUrl = model.PosterUrl;
                }

                if (model.TrailerUrl != anime.TrailerUrl)
                {
                    anime.TrailerUrl = UrlParser.ParseTrailerUrl(model.TrailerUrl);
                }

                _mapper.Map(model, anime);

                _unitOfWork.GetRepository<Anime>().Update(anime);
                _unitOfWork.SaveChanges();

                //Change genres
                var repo = _unitOfWork.GetRepository<AnimeGenre>();
                for (int i = 0; i < anime.AnimeGenres.Count; i++)
                {
                    repo.Delete(anime.AnimeGenres.ToList()[i]);
                }

                var newGenres = new List<AnimeGenre>();
                foreach (var genre in model.Genres)
                {
                    newGenres.Add(new AnimeGenre { AnimeId = anime.Id, GenreId = genre.Id });                                                       
                }
                repo.Insert(newGenres);
                _unitOfWork.SaveChanges();

                var response = _mapper.Map<AnimeResponseModel>(anime);
               
                return new BaseResponse<AnimeResponseModel>
                {
                    Data = response,
                    StatusCode = HttpStatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<AnimeResponseModel>
                {
                    Description = $"Error [EditAnime]: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public IBaseResponse<AnimeResponseModel> Get(int id)
        {   
            try
            {
                var anime = _unitOfWork.GetRepository<Anime>().GetFirstOrDefault(
                    predicate: x => x.Id == id,
                    include: i => i
                            .Include(x => x.AnimeGenres)
                                .ThenInclude(x => x.Genre)
                                    .ThenInclude(x => x.GenreName));

                if (anime == null)
                {
                    return new BaseResponse<AnimeResponseModel>
                    {
                        Description = "Anime not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                var response = _mapper.Map<Anime,AnimeResponseModel>(anime);

                return new BaseResponse<AnimeResponseModel>
                {
                    Data = response,
                    StatusCode = HttpStatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<AnimeResponseModel>
                {
                    Description = $"Error [GetAnime]: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public IBaseResponse<AnimeResponseModel> Create(AnimeRequestModel model)
        {
            try
            {   
                string posterUrl = AnimeConstans.POSTER_URL; //default poster
                string trailerUrl = null;

                if (model.PosterUrl != null)
                {
                    posterUrl = model.PosterUrl;
                }

                if(model.TrailerUrl != null)
                {
                    trailerUrl = UrlParser.ParseTrailerUrl(model.TrailerUrl);
                }

                var anime = new Anime()
                {                   
                    Title = model.Title,
                    Episodes = model.Episodes,
                    EpisodeDuration = model.EpisodeDuration,
                    AnimeType = model.AnimeType.ToEnum<AnimeType>(),
                    ReleaseDate = DateTime.ParseExact(model.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    PosterUrl = posterUrl,
                    TrailerUrl = trailerUrl
                };

                _unitOfWork.GetRepository<Anime>().Insert(anime);
                _unitOfWork.SaveChanges();

                var animeGenres = new List<AnimeGenre>();
                foreach(var animegenre in model.Genres)
                {
                    animeGenres.Add(
                        new AnimeGenre()
                        {
                            AnimeId = anime.Id,
                            GenreId = _unitOfWork.GetRepository<Genre>()
                                .GetFirstOrDefault(
                                    predicate:
                                        x => x.GenreName == animegenre.GenreName)!.Id
                        });
                }

                _unitOfWork.GetRepository<AnimeGenre>().Insert(animeGenres);
                _unitOfWork.SaveChanges();

                var respone = _mapper.Map<AnimeResponseModel>(anime);

                return new BaseResponse<AnimeResponseModel>
                {
                    Data = respone,
                    StatusCode = HttpStatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<AnimeResponseModel>
                {
                    Description = $"Error [CreateAnime]: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<List<AnimeResponseModel>>> GetAll(Filter filter)
        {
            try
            {
                Expression<Func<Anime, bool>>? predicate = null;
                Func<IQueryable<Anime>, IOrderedQueryable<Anime>>? orderBy = null;

                if (filter.SearchQuery != null)
                {
                    predicate = x => x.Title.Contains(filter.SearchQuery);                  
                }

                if (filter.AnimeType != null)
                {
                    var animeType = Enum.Parse<AnimeType>(filter.AnimeType);
                    Expression<Func<Anime, bool>> predicateAnimeType = x =>x.AnimeType == animeType;
                    predicate = predicate.And(predicateAnimeType);
                }

                if (filter.Genres != null)
                {
                    var genres = filter.Genres.Where(x => x.Checked == true).Select(x => x.Name).ToList();
                    if (genres.Count != 0)
                    {
                        Expression<Func<Anime, bool>> predicateGenres = a => a.AnimeGenres.Where(ag => genres.Contains(ag.Genre.GenreName)).Count() == genres.Count();
                        predicate =  predicate.And(predicateGenres);
                    }
                }

                if (filter.OrderBy != null)
                {
                    ParameterExpression param = Expression.Parameter(typeof(Anime), "a");
                    Expression property = null;

                    switch (filter.OrderBy)
                    {
                        case "Title":
                            property = Expression.Property(param, nameof(Anime.Title));
                            break;
                        case "Rating":
                            property = Expression.Property(param, nameof(Anime.Rating));
                            break;
                        case "RealizeDate":
                            property = Expression.Property(param, nameof(Anime.ReleaseDate));
                            break;
                    }
                    var lambda = Expression.Lambda<Func<Anime, object>>(Expression.Convert(property, typeof(object)), param);

                    if (filter.AscOrDesc == "ASC")
                    {
                        orderBy = x => x.OrderBy(lambda);
                    }
                    else if(filter.AscOrDesc == "DESC")
                    { 
                        orderBy = x => x.OrderByDescending(lambda);
                    }
                }
               
                var anime = await _unitOfWork.GetRepository<Anime>().GetAllAsync(
                    predicate: predicate,
                    include: x => x
                        .Include(x => x.AnimeGenres)
                            .ThenInclude(x => x.Genre),
                    orderBy: orderBy);
               
                var response = _mapper.Map<List<AnimeResponseModel>>(anime);

                return new BaseResponse<List<AnimeResponseModel>>
                {
                    Data = response,
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<AnimeResponseModel>>
                {
                    Description = $"Error [GetAllAnime]: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }
    }
}
