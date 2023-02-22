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

        public IBaseResponse<MessageResponseModel> Create(MessageRequestModel model, int userId)
        {
            try
            {           
                var userProfile = _unitOfWork.GetRepository<UserProfile>().GetFirstOrDefault(
                    predicate: x => x.UserId == userId,
                    include: i => i
                        .Include(x => x.FileModel));

                var message = _mapper.Map<Message>(model);
                message.AuthorId = userId;

                _unitOfWork.GetRepository<Message>().Insert(message);
                _unitOfWork.SaveChanges();

                var response = _mapper.Map<MessageResponseModel>(message);
                response.Author = userProfile.Name;
                response.AvatarUrl = userProfile.FileModel.Path;

                return new BaseResponse<MessageResponseModel>
                {
                    Data = response,
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<MessageResponseModel>
                {
                    Description = $"Error [CreateMessage]: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }
        public IBaseResponse<MessageResponseModel> Get(int id)
        {
            try
            {
                var news = _unitOfWork.GetRepository<Message>().GetFirstOrDefault(
                predicate: x => x.Id == id,
                include: i => i
                    .Include(x => x.Author)
                        .ThenInclude(x => x.Profile));

                if (news == null)
                {
                    return new BaseResponse<MessageResponseModel>
                    {
                        Description = "News not found",
                        StatusCode = HttpStatusCode.NotFound
                    };
                }

                var response = _mapper.Map<MessageResponseModel>(news);

                return new BaseResponse<MessageResponseModel>
                {
                    Data = response,
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<MessageResponseModel>
                {
                    Description = $"Error [GetMessage]: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }
        public async Task<IBaseResponse<List<MessageResponseModel>>> GetChatHistory(int pageIndex, int pageSize)
        {
            try
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

                var response = _mapper.Map<List<MessageResponseModel>>(messageList.Items);

                return new BaseResponse<List<MessageResponseModel>>
                {
                    Data = response,
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<MessageResponseModel>>
                {
                    Description = $"Error [GetChatHistory]: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }
        public IBaseResponse<bool> Delete(int id)
        {
            try
            {            
                _unitOfWork.GetRepository<Message>().Delete(id);
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
                    Description = $"Error [DeleteMessage]: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

    }
}
