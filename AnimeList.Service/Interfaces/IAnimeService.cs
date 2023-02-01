using AnimeList.Domain.Entity.Animes;
using AnimeList.Domain.Entity.Genres;
using AnimeList.Domain.Enum;
using AnimeList.Domain.RequestModels;
using AnimeList.Domain.RequestModels.SearchAnime;
using AnimeList.Domain.Response;
using AnimeList.Domain.ResponseModel;
using System.Net;

namespace AnimeList.Services.Interfaces
{
    public interface IAnimeService
    {
        IBaseResponse<bool> Delete(int id);
        IBaseResponse<AnimeResponseModel> Edit(AnimeRequestModel model);
        IBaseResponse<AnimeResponseModel> Get(int id);
        IBaseResponse<AnimeResponseModel> Create(AnimeRequestModel model);
        Task<IBaseResponse<List<AnimeResponseModel>>> GetAll(Filter filter);
    }
}
