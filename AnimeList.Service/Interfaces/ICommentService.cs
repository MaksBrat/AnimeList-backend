using AnimeList.Domain.RequestModels.AnimeNews;
using AnimeList.Domain.Response;
using AnimeList.Domain.ResponseModels.AnimeNews;

namespace AnimeList.Services.Interfaces
{
    public interface ICommentService
    {
        public IBaseResponse<CommentResponseModel> Create(CommentRequestModel model,int userId);
        public IBaseResponse<CommentResponseModel> Get(int id);
        public Task<IBaseResponse<List<CommentResponseModel>>> GetAll(int newdId);
        public IBaseResponse<CommentResponseModel> Edit(CommentRequestModel model);     
        public IBaseResponse<bool> Delete(int id);
    }
}
