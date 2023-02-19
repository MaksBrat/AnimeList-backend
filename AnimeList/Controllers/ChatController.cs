using AnimeList.Common.Extentions;
using AnimeList.Domain.RequestModels.Chat;
using AnimeList.Hubs;
using AnimeList.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Net;

namespace AnimeList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IMessageService _messageService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private int _userId;

        public ChatController(IHubContext<ChatHub> hubContext, IMessageService messageService, IHttpContextAccessor httpContextAccessor)
        {
            _hubContext = hubContext;
            _messageService = messageService;
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

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] MessageRequestModel model)
        {
            var response = _messageService.Create(model, _userId);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", response.Data);
                return Ok();
            }
            return new BadRequestObjectResult(new { Message = response.Description });
        }

        [HttpGet("getChatMessages")]
        public async Task<IActionResult> getChatMessages([FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            var response = await _messageService.GetChatHistory(pageIndex, pageSize);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Data);
            }
            return new BadRequestObjectResult(new { Message = response.Description });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var response = _messageService.Delete(id);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                await _hubContext.Clients.All.SendAsync("MessageDeleted", id);
                return Ok();
            }
            return new BadRequestObjectResult(new { Message = response.Description });
        }
    }
}
