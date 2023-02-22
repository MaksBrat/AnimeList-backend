using AnimeList.Domain.RequestModels;
using AnimeList.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using AnimeList.Domain.RequestModels.AnimeNews;
using AnimeList.Common.Extentions;
using Microsoft.AspNetCore.Authorization;

namespace AnimeList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private int _userId;

        public CommentController(ICommentService commentService, IHttpContextAccessor httpContextAccessor)
        {
            _commentService = commentService;
            _httpContextAccessor = httpContextAccessor;

            var userId = _httpContextAccessor.HttpContext.User.GetUserId();
            if (userId != null)
            {
                _userId = userId;
            }
            else
            {
                _userId = 0;
            }   
        }

        [Authorize]
        [HttpPost("create")]
        public IActionResult Create([FromBody] CommentRequestModel model)
        {
            var response = _commentService.Create(model, _userId);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Data);
            }
            return new BadRequestObjectResult(new { Message = response.Description });
        }

        [HttpPost("get/{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            var response = _commentService.Get(id);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Data);
            }
            return new BadRequestObjectResult(new { Message = response.Description });
        }

        [HttpGet("getAll/{newsId}")]
        public async Task<IActionResult> getAll([FromRoute] int newsId)
        {
            var response = await _commentService.GetAll(newsId);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Data);
            }
            return new BadRequestObjectResult(new { Message = response.Description });
        }

        [Authorize]
        [HttpPost("edit")]
        public IActionResult Edit([FromForm] CommentRequestModel model)
        {
            var response = _commentService.Edit(model);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Data);
            }
            return new BadRequestObjectResult(new { Message = response.Description });
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var response = _commentService.Delete(id);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Data);
            }
            return new BadRequestObjectResult(new { Message = response.Description });
        }
    }
}
