using AnimeList.Common.Filters;
using AnimeList.Domain.RequestModels;
using AnimeList.Domain.Response;
using AnimeList.Domain.ResponseModel;

namespace AnimeList.Services.Interfaces
{
    public interface IAnimeService
    {
        IBaseResponse<AnimeResponseModel> Create(AnimeRequestModel model);
        IBaseResponse<AnimeResponseModel> Get(int id);
        Task<IBaseResponse<List<AnimeResponseModel>>> GetAll(AnimeFilter filter);
        IBaseResponse<AnimeResponseModel> Edit(AnimeRequestModel model);
        IBaseResponse<bool> Delete(int id);             
    }
}
