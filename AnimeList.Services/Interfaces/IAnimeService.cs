using AnimeList.Domain.RequestModels;
using AnimeList.Domain.RequestModels.EntitiesFilters;
using AnimeList.Domain.Response;
using AnimeList.Domain.ResponseModel;

namespace AnimeList.Services.Interfaces
{
    public interface IAnimeService
    {
        IBaseResponse<AnimeResponse> Create(AnimeRequest model);
        IBaseResponse<AnimeResponse> Get(int id);
        Task<IBaseResponse<List<AnimeResponse>>> GetAll(AnimeFilterRequest filterRequest);
        IBaseResponse<AnimeResponse> Edit(AnimeRequest model);
        IBaseResponse<bool> Delete(int id);             
    }
}
