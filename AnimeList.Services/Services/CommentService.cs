using AnimeList.DAL.Interfaces;
using AnimeList.Domain.Entity.Account;
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

        public IBaseResponse<CommentResponse> Create(CommentRequest model, int userId)
        {
                var userProfile = _unitOfWork.GetRepository<UserProfile>().GetFirstOrDefault(
                    predicate: x => x.UserId == userId,
                    include: i => i
                        .Include(x => x.FileModel));

                var comment = _mapper.Map<Comment>(model);
                comment.AuthorId = userId;

                _unitOfWork.GetRepository<Comment>().Insert(comment);
                _unitOfWork.SaveChanges();

                var response = _mapper.Map<CommentResponse>(comment);
                response.Author = userProfile.Name;
                response.AvatarUrl = userProfile.FileModel.Path;

                return new BaseResponse<CommentResponse>
                {
                    Data = response,
                    StatusCode = HttpStatusCode.OK
                };
        }
        public IBaseResponse<CommentResponse> Get(int id)
        {
                var comment = _unitOfWork.GetRepository<Comment>().GetFirstOrDefault(
                    predicate: x => x.Id == id);

                if (comment == null)
                {
                    return new BaseResponse<CommentResponse>
                    {
                        Description = "News not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                var response = _mapper.Map<CommentResponse>(comment);

                return new BaseResponse<CommentResponse>
                {
                    Data = response,
                    StatusCode = HttpStatusCode.OK
                };
        }
        public async Task<IBaseResponse<List<CommentResponse>>> GetAll(int newdId)
        {
            var comments = await _unitOfWork.GetRepository<Comment>().GetAllAsync(
                predicate: x => x.NewsId == newdId,
                include: i => i
                    .Include(x => x.Author.Profile.FileModel),
                orderBy: x => x.OrderByDescending(x => x.DateCreated));

            if (comments == null)
            {
                return new BaseResponse<List<CommentResponse>>
                {
                    Description = "Commets not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            var response = _mapper.Map<List<CommentResponse>>(comments);

            return new BaseResponse<List<CommentResponse>>
            {
                Data = response,
                StatusCode = HttpStatusCode.OK
            };
        }
        public IBaseResponse<CommentResponse> Edit(CommentRequest model)
        {
            var comment = _unitOfWork.GetRepository<News>().GetFirstOrDefault(
                predicate: x => x.Id == model.Id);

            if (comment == null)
            {
                return new BaseResponse<CommentResponse>
                {
                    Description = "Comment not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            _mapper.Map(model, comment);

            _unitOfWork.GetRepository<News>().Update(comment);
            _unitOfWork.SaveChanges();

            var response = _mapper.Map<CommentResponse>(comment);

            return new BaseResponse<CommentResponse>
            {
                Data = response,
                StatusCode = HttpStatusCode.OK
            };
        }      
        public IBaseResponse<bool> Delete(int id)
        {
            _unitOfWork.GetRepository<Comment>().Delete(id);
            _unitOfWork.SaveChanges();

            return new BaseResponse<bool>
            {
                Data = true,
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}
