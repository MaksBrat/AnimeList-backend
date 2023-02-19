
using AnimeList.Domain.RequestModels;
using AnimeList.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using AnimeList.Domain.RequestModels.AnimeNews;
using AnimeList.Common.Extentions;

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
                _userId = Int32.Parse(userId);
            }
            else
            {
                _userId = 0;
            }   
        }

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
