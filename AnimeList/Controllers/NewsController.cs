
using AnimeList.Domain.RequestModels;
using AnimeList.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using AnimeList.Domain.RequestModels.AnimeNews;
using AnimeList.Common.Extentions;
using AnimeList.Common.Filters;

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

            var userId = _httpContextAccessor.HttpContext.User.GetUserId();
            if (userId != null)
            {
                _userId = Int32.Parse(userId);
            }
            else
            {
                _userId = 0;
            }
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllNewsWithComments([FromQuery] NewsFilter filter)
        {
            var response = await _newsService.GetAllAsync(filter);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Data);
            }
            return new BadRequestObjectResult(new { Message = response.Description });
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] NewsRequestModel model)
        {
            var response = _newsService.Create(model, _userId);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Data);
            }
            return new BadRequestObjectResult(new { Message = response.Description });
        }

        [HttpPost("get/{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            var response = _newsService.Get(id);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Data);
            }
            return new BadRequestObjectResult(new { Message = response.Description });
        }

        [HttpPost("edit")]
        public IActionResult Edit([FromForm] NewsRequestModel model)
        {
            var response = _newsService.Edit(model);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Data);
            }
            return new BadRequestObjectResult(new { Message = response.Description });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var response = await _newsService.DeleteAsync(id);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Data);
            }
            return new BadRequestObjectResult(new { Message = response.Description });
        }
    }
}
