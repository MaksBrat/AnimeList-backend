using AnimeList.DAL.Interfaces;
using AnimeList.Domain.Chat;
using AnimeList.Domain.Entity.Account;
using AnimeList.Domain.RequestModels.Chat;
using AnimeList.Domain.Response;
using AnimeList.Domain.ResponseModels.Chat;
using AnimeList.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace AnimeList.Services.Services
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MessageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork; 
            _mapper = mapper;
        }

        public IBaseResponse<MessageResponse> Create(MessageRequest model, int userId)
        {        
            var userProfile = _unitOfWork.GetRepository<UserProfile>().GetFirstOrDefault(
                predicate: x => x.UserId == userId,
                include: i => i
                    .Include(x => x.FileModel));

            var message = _mapper.Map<Message>(model);
            message.AuthorId = userId;

            _unitOfWork.GetRepository<Message>().Insert(message);
            _unitOfWork.SaveChanges();

            var response = _mapper.Map<MessageResponse>(message);
            response.Author = userProfile.Name;
            response.AvatarUrl = userProfile.FileModel.Path;

            return new BaseResponse<MessageResponse>
            {
                Data = response,
                StatusCode = HttpStatusCode.OK
            };
        }
        public IBaseResponse<MessageResponse> Get(int id)
        {
            var news = _unitOfWork.GetRepository<Message>().GetFirstOrDefault(
            predicate: x => x.Id == id,
            include: i => i
                .Include(x => x.Author)
                    .ThenInclude(x => x.Profile));

            if (news == null)
            {
                return new BaseResponse<MessageResponse>
                {
                    Description = "News not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            var response = _mapper.Map<MessageResponse>(news);

            return new BaseResponse<MessageResponse>
            {
                Data = response,
                StatusCode = HttpStatusCode.OK
            };
        }
        public async Task<IBaseResponse<List<MessageResponse>>> GetChatHistory(int pageIndex, int pageSize)
        {
            var messageList = await _unitOfWork.GetRepository<Message>().GetPagedListAsync(
                orderBy: x => x.OrderByDescending(x => x.DateCreated),
                include: x => x
                    .Include(x => x.Author)
                        .ThenInclude(x => x.Profile)
                            .ThenInclude(x => x.FileModel),
                pageIndex: pageIndex,
                pageSize: pageSize
            );

            var response = _mapper.Map<List<MessageResponse>>(messageList.Items);

            return new BaseResponse<List<MessageResponse>>
            {
                Data = response,
                StatusCode = HttpStatusCode.OK
            };
        }
        public IBaseResponse<bool> Delete(int id)
        {        
            _unitOfWork.GetRepository<Message>().Delete(id);
            _unitOfWork.SaveChanges();

            return new BaseResponse<bool>
            {
                Data = true,
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}
