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
        public Task<IBaseResponse<List<NewsResponseModel>>> GetAll(NewsFilter filter);    
        public Task<IBaseResponse<bool>> Delete(int id);
    }
}
