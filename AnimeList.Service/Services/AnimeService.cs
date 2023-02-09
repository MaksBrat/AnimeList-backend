using AnimeList.Common.Filters;
using AnimeList.DAL.Interfaces;
using AnimeList.Domain.Entity.Animes;
using AnimeList.Domain.Entity.Genres;
using AnimeList.Domain.RequestModels;
using AnimeList.Domain.Response;
using AnimeList.Domain.ResponseModel;
using AnimeList.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
             
        public IBaseResponse<AnimeResponseModel> Create(AnimeRequestModel model)
        {
            try
            {
                var anime = _mapper.Map<Anime>(model);

                _unitOfWork.GetRepository<Anime>().Insert(anime);
                _unitOfWork.SaveChanges();

                var animeGenres = new List<AnimeGenre>();
                foreach (var animegenre in model.Genres)
                {
                    animeGenres.Add(
                        new AnimeGenre()
                        {
                            AnimeId = anime.Id,
                            GenreId = _unitOfWork.GetRepository<Genre>()
                                .GetFirstOrDefault(
                                    predicate:
                                        x => x.Name == animegenre.Name)!.Id
                        });
                }

                _unitOfWork.GetRepository<AnimeGenre>().Insert(animeGenres);
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
                    Description = $"Error [CreateAnime]: {ex.Message}",
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
                                    .ThenInclude(x => x.Name));

                if (anime == null)
                {
                    return new BaseResponse<AnimeResponseModel>
                    {
                        Description = "Anime not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

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
                    Description = $"Error [GetAnime]: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }
        public async Task<IBaseResponse<List<AnimeResponseModel>>> GetAll(AnimeFilter filter)
        {
            try
            {
                filter.Filter();

                var anime = await _unitOfWork.GetRepository<Anime>().GetAllAsync(
                    predicate: filter.Predicate,
                    include: x => x
                        .Include(x => x.AnimeGenres)
                            .ThenInclude(x => x.Genre),
                    orderBy: filter.OrderByQuery,
                    take: filter.Take);

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

                _mapper.Map(model, anime);

                //Change genres
                var repo = _unitOfWork.GetRepository<AnimeGenre>();
                for (int i = 0; i < anime.AnimeGenres.Count; i++)
                {
                    repo.Delete(anime.AnimeGenres.ToList()[i]);
                }

                var newGenres = new List<AnimeGenre>();
                foreach (var genre in model.Genres)
                {
                    var Genre = _unitOfWork.GetRepository<Genre>().GetFirstOrDefault(
                        predicate: x => x.Id == genre.Id);

                    newGenres.Add(new AnimeGenre { AnimeId = anime.Id, GenreId = genre.Id, Genre = Genre });
                }
                repo.Insert(newGenres);

                _unitOfWork.GetRepository<Anime>().Update(anime);
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
    }
}
