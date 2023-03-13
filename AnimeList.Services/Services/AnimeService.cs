using AnimeList.Common.EntitiesFilters;
using AnimeList.DAL.Interfaces;
using AnimeList.Domain.Entity.Animes;
using AnimeList.Domain.Entity.Genres;
using AnimeList.Domain.RequestModels;
using AnimeList.Domain.RequestModels.EntitiesFilters;
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
             
        public IBaseResponse<AnimeResponse> Create(AnimeRequest model)
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

            var response = _mapper.Map<AnimeResponse>(anime);

            return new BaseResponse<AnimeResponse>
            {
                Data = response,
                StatusCode = HttpStatusCode.OK
            };       
        }
        public IBaseResponse<AnimeResponse> Get(int id)
        {   
            var anime = _unitOfWork.GetRepository<Anime>().GetFirstOrDefault(
                predicate: x => x.Id == id,
                include: i => i
                        .Include(x => x.AnimeGenres)
                            .ThenInclude(x => x.Genre));

            if (anime == null)
            {
                return new BaseResponse<AnimeResponse>
                {
                    Description = "Anime not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            var response = _mapper.Map<AnimeResponse>(anime);

            return new BaseResponse<AnimeResponse>
            {
                Data = response,
                StatusCode = HttpStatusCode.OK
            };
        }
        public async Task<IBaseResponse<List<AnimeResponse>>> GetAll(AnimeFilterRequest filterRequest)
        {
            var filter = _mapper.Map<AnimeFilter>(filterRequest);

            filter.CreateFilter();

            var anime = await _unitOfWork.GetRepository<Anime>().GetAllAsync(
                predicate: filter.Predicate,
                include: x => x
                    .Include(x => x.AnimeGenres)
                        .ThenInclude(x => x.Genre),
                orderBy: filter.OrderByQuery,
                take: filter.Take);

            var response = _mapper.Map<List<AnimeResponse>>(anime);

            return new BaseResponse<List<AnimeResponse>>
            {
                Data = response,
                StatusCode = HttpStatusCode.OK
            };
        }
        public IBaseResponse<AnimeResponse> Edit(AnimeRequest model)
        {
            var anime = _unitOfWork.GetRepository<Anime>().GetFirstOrDefault(
                predicate: x => x.Id == model.Id,
                include: i => i
                        .Include(x => x.AnimeGenres)
                            .ThenInclude(x => x.Genre));

            if (anime == null)
            {
                return new BaseResponse<AnimeResponse>
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

            var response = _mapper.Map<AnimeResponse>(anime);

            return new BaseResponse<AnimeResponse>
            {
                Data = response,
                StatusCode = HttpStatusCode.OK
            };
        }
        public IBaseResponse<bool> Delete(int id)
        {
            _unitOfWork.GetRepository<Anime>().Delete(id);
            _unitOfWork.SaveChanges();

            return new BaseResponse<bool>
            {
                Data = true,
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}
