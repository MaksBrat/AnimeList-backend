using AnimeList.Common.Filters;
using AnimeList.Domain.RequestModels.AnimeNews;
using AnimeList.Domain.Response;
using AnimeList.Domain.ResponseModels.AnimeNews;

namespace AnimeList.Services.Interfaces
{
    public interface INewsService
    {
        public IBaseResponse<NewsResponseModel> Create(NewsRequestModel model, int userId);
        public IBaseResponse<NewsResponseModel> Get(int id);
        public Task<IBaseResponse<List<NewsResponseModel>>> GetAllAsync(NewsFilter filter);
        public IBaseResponse<NewsResponseModel> Edit(NewsRequestModel model);      
        public Task<IBaseResponse<bool>> DeleteAsync(int id);
    }
}
