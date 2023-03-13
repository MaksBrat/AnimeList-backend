using AnimeList.Domain.RequestModels.AnimeNews;
using AnimeList.Domain.Response;
using AnimeList.Domain.ResponseModels.AnimeNews;

namespace AnimeList.Services.Interfaces
{
    public interface ICommentService
    {
        public IBaseResponse<CommentResponse> Create(CommentRequest model,int userId);
        public IBaseResponse<CommentResponse> Get(int id);
        public Task<IBaseResponse<List<CommentResponse>>> GetAll(int newdId);
        public IBaseResponse<CommentResponse> Edit(CommentRequest model);     
        public IBaseResponse<bool> Delete(int id);
    }
}
