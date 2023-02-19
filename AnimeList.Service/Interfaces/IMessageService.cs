﻿using AnimeList.Domain.Chat;
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
        public IBaseResponse<MessageResponseModel> Create(MessageRequestModel model, int userId);
        public IBaseResponse<MessageResponseModel> Get(int id);
        public Task<IBaseResponse<List<MessageResponseModel>>> GetChatHistory(int pageIndex, int pageSize);
        public IBaseResponse<bool> Delete(int id);
    }
}
