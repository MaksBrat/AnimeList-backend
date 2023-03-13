using AnimeList.Domain.Chat;
using AnimeList.Domain.Pagination;
using AnimeList.Domain.RequestModels.Chat;
using AnimeList.Domain.Response;
using AnimeList.Domain.ResponseModels.Chat;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AnimeList.Services.Interfaces
{
    public interface IMessageService
    {
        public IBaseResponse<MessageResponse> Create(MessageRequest model, int userId);
        public IBaseResponse<MessageResponse> Get(int id);
        public Task<IBaseResponse<List<MessageResponse>>> GetChatHistory(int pageIndex, int pageSize);
        public IBaseResponse<bool> Delete(int id);
    }
}
