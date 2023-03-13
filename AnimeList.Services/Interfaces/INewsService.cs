using AnimeList.Domain.RequestModels.AnimeNews;
using AnimeList.Domain.RequestModels.EntitiesFilters;
using AnimeList.Domain.Response;
using AnimeList.Domain.ResponseModels.AnimeNews;

namespace AnimeList.Services.Interfaces
{
    public interface INewsService
    {
        public IBaseResponse<NewsResponse> Create(NewsRequest model, int userId);
        public IBaseResponse<NewsResponse> Get(int id);
        public Task<IBaseResponse<List<NewsResponse>>> GetAll(NewsFilterRequest filter);    
        public Task<IBaseResponse<bool>> Delete(int id);
    }
}
