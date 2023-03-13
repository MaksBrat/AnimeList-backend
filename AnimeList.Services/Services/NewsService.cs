using AnimeList.DAL.Interfaces;
using AnimeList.Domain.Entity.AnimeNews;
using AnimeList.Domain.RequestModels.AnimeNews;
using AnimeList.Domain.Response;
using AnimeList.Domain.ResponseModels.AnimeNews;
using AnimeList.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Net;
using AnimeList.Domain.Entity.Account;
using AnimeList.Common.EntitiesFilters;
using AnimeList.Domain.RequestModels.EntitiesFilters;

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

        public IBaseResponse<NewsResponse> Create(NewsRequest model, int userId)
        {
            var userProfile = _unitOfWork.GetRepository<UserProfile>().GetFirstOrDefault(
                    predicate: x => x.UserId == userId,
                    include: i => i
                        .Include(x => x.FileModel));

            var news = _mapper.Map<News>(model);
            news.AuthorId = userId;

            _unitOfWork.GetRepository<News>().Insert(news);
            _unitOfWork.SaveChanges();
             
            var response = _mapper.Map<NewsResponse>(news);
            response.Author = userProfile.Name;
            response.AvatarUrl = userProfile.FileModel.Path;

            return new BaseResponse<NewsResponse>
            {
                Data = response,
                StatusCode = HttpStatusCode.OK
            };
        }
        public IBaseResponse<NewsResponse> Get(int id)
        {
            var news = _unitOfWork.GetRepository<News>().GetFirstOrDefault(
            predicate: x => x.Id == id,
            include: i => i
                .Include(x => x.Author)
                    .ThenInclude(x => x.Profile)
                        .ThenInclude(x => x.FileModel));

            if (news == null)
            {
                return new BaseResponse<NewsResponse>
                {
                    Description = "News not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            var response = _mapper.Map<NewsResponse>(news);

            return new BaseResponse<NewsResponse>
            {
                Data = response,
                StatusCode = HttpStatusCode.OK
            };
        }
        public async Task<IBaseResponse<List<NewsResponse>>> GetAll(NewsFilterRequest filterRequest)
        {
            var filter = _mapper.Map<NewsFilter>(filterRequest);

            filter.CreateFilter();

            var news = await _unitOfWork.GetRepository<News>().GetAllAsync(
                predicate: filter.Predicate,
                include: x => x
                    .Include(x => x.Author)
                        .ThenInclude(x => x.Profile)
                            .ThenInclude(x => x.FileModel),
                orderBy: filter.OrderByQuery,
                take: filter.Take);

            var response = _mapper.Map<List<NewsResponse>>(news);

            return new BaseResponse<List<NewsResponse>>
            {
                Data = response,
                StatusCode = HttpStatusCode.OK
            };
        }
        public IBaseResponse<NewsResponse> Edit(NewsRequest model)
        {
            var news = _unitOfWork.GetRepository<News>().GetFirstOrDefault(
                predicate: x => x.Id == model.Id);
                   
            if (news == null)
            {
                return new BaseResponse<NewsResponse>
                {
                    Description = "News not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            _mapper.Map(model, news);

            _unitOfWork.GetRepository<News>().Update(news);
            _unitOfWork.SaveChanges();

            var response = _mapper.Map<NewsResponse>(news);

            return new BaseResponse<NewsResponse>
            {
                Data = response,
                StatusCode = HttpStatusCode.OK
            };
        }                 
        public async Task<IBaseResponse<bool>> Delete(int id)
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
    }
}
