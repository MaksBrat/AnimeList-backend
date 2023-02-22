using AnimeList.DAL.Interfaces;
using AnimeList.Domain.Entity.AnimeNews;
using AnimeList.Domain.RequestModels.AnimeNews;
using AnimeList.Domain.Response;
using AnimeList.Domain.ResponseModels.AnimeNews;
using AnimeList.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Net;
using AnimeList.Common.Filters;
using AnimeList.Domain.Entity.Account;

namespace AnimeList.Services.Services
{
    public class NewsService : INewsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NewsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IBaseResponse<NewsResponseModel> Create(NewsRequestModel model, int userId)
        {
			try
			{
                var userProfile = _unitOfWork.GetRepository<UserProfile>().GetFirstOrDefault(
                     predicate: x => x.UserId == userId,
                     include: i => i
                         .Include(x => x.FileModel));

                var news = _mapper.Map<News>(model);
                news.AuthorId = userId;

                _unitOfWork.GetRepository<News>().Insert(news);
                _unitOfWork.SaveChanges();
             
                var response = _mapper.Map<NewsResponseModel>(news);
                response.Author = userProfile.Name;
                response.AvatarUrl = userProfile.FileModel.Path;

                return new BaseResponse<NewsResponseModel>
                {
                    Data = response,
                    StatusCode = HttpStatusCode.OK
                };
            }
			catch (Exception ex)
			{
                return new BaseResponse<NewsResponseModel>
                {
                    Description = $"Error [CreateNews]: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }
        public IBaseResponse<NewsResponseModel> Get(int id)
        {
            try
            {
                var news = _unitOfWork.GetRepository<News>().GetFirstOrDefault(
                predicate: x => x.Id == id,
                include: i => i
                    .Include(x => x.Author)
                        .ThenInclude(x => x.Profile)
                            .ThenInclude(x => x.FileModel));

                if (news == null)
                {
                    return new BaseResponse<NewsResponseModel>
                    {
                        Description = "News not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                var response = _mapper.Map<NewsResponseModel>(news);

                return new BaseResponse<NewsResponseModel>
                {
                    Data = response,
                    StatusCode = HttpStatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<NewsResponseModel>
                {
                    Description = $"Error [GetNews]: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }

        }
        public async Task<IBaseResponse<List<NewsResponseModel>>> GetAll(NewsFilter filter)
        {
            try
            {
                filter.Filter();

                var news = await _unitOfWork.GetRepository<News>().GetAllAsync(
                    predicate: filter.Predicate,
                    include: x => x
                        .Include(x => x.Author)
                            .ThenInclude(x => x.Profile)
                                .ThenInclude(x => x.FileModel),
                    orderBy: filter.OrderByQuery,
                    take: filter.Take);

                var response = _mapper.Map<List<NewsResponseModel>>(news);

                return new BaseResponse<List<NewsResponseModel>>
                {
                    Data = response,
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<NewsResponseModel>>
                {
                    Description = $"Error [GetAllNews]: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }
        public IBaseResponse<NewsResponseModel> Edit(NewsRequestModel model)
        {
            try
            {
                var news = _unitOfWork.GetRepository<News>().GetFirstOrDefault(
                    predicate: x => x.Id == model.Id);
                   
                if (news == null)
                {
                    return new BaseResponse<NewsResponseModel>
                    {
                        Description = "News not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                _mapper.Map(model, news);

                _unitOfWork.GetRepository<News>().Update(news);
                _unitOfWork.SaveChanges();

                var response = _mapper.Map<NewsResponseModel>(news);

                return new BaseResponse<NewsResponseModel>
                {
                    Data = response,
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<NewsResponseModel>
                {
                    Description = $"Error [EditNews]: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }                 
        public async Task<IBaseResponse<bool>> Delete(int id)
        {
            try
            {
                var comments = await _unitOfWork.GetRepository<Comment>().GetAllAsync(
                    predicate: x => x.NewsId == id);

                foreach(var comment in comments)
                {
                    _unitOfWork.GetRepository<Comment>().Delete(comment);
                }

                _unitOfWork.GetRepository<News>().Delete(id);
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
