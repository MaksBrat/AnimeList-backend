using AnimeList.DAL.Interfaces;
using AnimeList.Domain.Chat;
using AnimeList.Domain.Entity.AnimeNews;
using AnimeList.Domain.RequestModels.Chat;
using AnimeList.Domain.Response;
using AnimeList.Domain.ResponseModels.Chat;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace AnimeList.Services.Services
{
    public class MessageService
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
                var message = _mapper.Map<Message>(model);
                message.AuthorId = userId;

                _unitOfWork.GetRepository<Message>().Insert(message);
                _unitOfWork.SaveChanges();

                var response = _mapper.Map<MessageResponseModel>(message);

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
        /*public IBaseResponse<List<MessageResponseModel>> GetAll()
        {
            try
            {

            }
            catch (Exception ex)
            {
                return new BaseResponse<List<MessageResponseModel>>
                {
                    Description = $"Error [GetAllMessage]: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }*/
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
