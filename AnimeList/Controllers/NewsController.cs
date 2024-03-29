﻿using AnimeList.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using AnimeList.Domain.RequestModels.AnimeNews;
using AnimeList.Common.Extentions;
using Microsoft.AspNetCore.Authorization;
using AnimeList.Common.EntitiesFilters;
using AnimeList.Domain.RequestModels.EntitiesFilters;

namespace AnimeList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private int _userId;

        public NewsController(INewsService newsService, IHttpContextAccessor httpContextAccessor)
        {
            _newsService = newsService;
            _httpContextAccessor = httpContextAccessor;

            _userId = _httpContextAccessor.HttpContext.User?.GetUserId() ?? 0;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllNewsWithComments([FromQuery] NewsFilterRequest filterRequest)
        {
            var response = await _newsService.GetAll(filterRequest);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Data);
            }
            return new BadRequestObjectResult(new { Message = response.Description });
        }

        [Authorize]
        [HttpPost("create")]
        public IActionResult Create([FromBody] NewsRequest model)
        {
            var response = _newsService.Create(model, _userId);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Data);
            }
            return new BadRequestObjectResult(new { Message = response.Description });
        }

        [HttpGet("get/{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            var response = _newsService.Get(id);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Data);
            }
            return new BadRequestObjectResult(new { Message = response.Description });
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var response = await _newsService.Delete(id);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Data);
            }
            return new BadRequestObjectResult(new { Message = response.Description });
        }
    }
}
