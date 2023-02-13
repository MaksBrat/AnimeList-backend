using AnimeList.Domain.RequestModels.Chat;
using AnimeList.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace AnimeList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task<IActionResult> Send([FromBody] MessageRequestModel model)
        {
            await _hubContext.Clients.All.SendAsync("Receive",model.Text);
            return Ok();
        }

    }
}
