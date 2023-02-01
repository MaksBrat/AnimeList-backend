using AnimeList.Domain.RequestModels;
using AnimeList.Services.Interfaces;
using AnimeList.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace AnimeList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IJWTService _jwtService;

        public TokenController(IJWTService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh(TokenRequestModel model)
        {
            var _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var Name = User.Identity.Name;
            var response = await _jwtService.RefreshToken(model);
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(response.Data);
            }
            return BadRequest("Invalid client request");
        }
    }
}
