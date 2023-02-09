using AnimeList.DAL.Interfaces;
using AnimeList.Domain.Entity.AnimeNews;
using AnimeList.Domain.RequestModels.AnimeNews;
using AnimeList.Domain.Response;
using AnimeList.Domain.ResponseModels.AnimeNews;
using AnimeList.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;


namespace AnimeList.Services.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CommentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IBaseResponse<CommentResponseModel> Create(CommentRequestModel model, int userId)
        {
            try
            {
                var comment = _mapper.Map<Comment>(model);

                comment.AuthorId = userId;

                _unitOfWork.GetRepository<Comment>().Insert(comment);
                _unitOfWork.SaveChanges();

                var response = _mapper.Map<CommentResponseModel>(comment);

                return new BaseResponse<CommentResponseModel>
                {
                    Data = response,
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<CommentResponseModel>
                {
                    Description = $"Error [CreateComment]: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }
        public IBaseResponse<CommentResponseModel> Get(int id)
        {
            try
            {
                var comment = _unitOfWork.GetRepository<Comment>().GetFirstOrDefault(
                    predicate: x => x.Id == id);

                if (comment == null)
                {
                    return new BaseResponse<CommentResponseModel>
                    {
                        Description = "News not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                var response = _mapper.Map<CommentResponseModel>(comment);

                return new BaseResponse<CommentResponseModel>
                {
                    Data = response,
                    StatusCode = HttpStatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<CommentResponseModel>
                {
                    Description = $"Error [GetComment]: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }
        public async Task<IBaseResponse<List<CommentResponseModel>>> GetAll(int newdId)
        {
            try
            {
                var comments = await _unitOfWork.GetRepository<Comment>().GetAllAsync(
                    predicate: x => x.NewsId == newdId,
                    include: i => i
                        .Include(x => x.Author.Profile),
                    orderBy: x => x.OrderByDescending(x => x.DateCreated));

                if (comments == null)
                {
                    return new BaseResponse<List<CommentResponseModel>>
                    {
                        Description = "Commets not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                var response = _mapper.Map<List<CommentResponseModel>>(comments);

                return new BaseResponse<List<CommentResponseModel>>
                {
                    Data = response,
                    StatusCode = HttpStatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<List<CommentResponseModel>>
                {
                    Description = $"Error [GetAllComments]: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }
        public IBaseResponse<CommentResponseModel> Edit(CommentRequestModel model)
        {
            try
            {
                var comment = _unitOfWork.GetRepository<News>().GetFirstOrDefault(
                    predicate: x => x.Id == model.Id);

                if (comment == null)
                {
                    return new BaseResponse<CommentResponseModel>
                    {
                        Description = "Comment not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                _mapper.Map(model, comment);

                _unitOfWork.GetRepository<News>().Update(comment);
                _unitOfWork.SaveChanges();

                var response = _mapper.Map<CommentResponseModel>(comment);

                return new BaseResponse<CommentResponseModel>
                {
                    Data = response,
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<CommentResponseModel>
                {
                    Description = $"Error [EditComment]: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }      
        public IBaseResponse<bool> Delete(int id)
        {
            try
            {
                _unitOfWork.GetRepository<Comment>().Delete(id);
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
