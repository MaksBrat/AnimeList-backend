using AnimeList.Common.Extentions;
using AnimeList.Domain.RequestModels;
using AnimeList.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AnimeList.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private int _userId;

        public ProfileController(IProfileService profileService,
            IHttpContextAccessor httpContextAccessor)
        {
            _profileService = profileService;
            _httpContextAccessor = httpContextAccessor;

            _userId = _httpContextAccessor.HttpContext.User?.GetUserId() ?? 0;
        }

        [HttpPost("edit")]
        public IActionResult Edit([FromForm] ProfileRequest profile)
        {          
            var response = _profileService.Edit(profile, _userId);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(new { Message = "Edit profile Successfully" });
            }                      
            return new BadRequestObjectResult(new { Message = response.Description });
        }

        [HttpPost("change-avatar")]
        public async Task<IActionResult> ChangeAvatar([FromForm] IFormFile avatar)
        {
            var response = await _profileService.ChangeAvatar(avatar, _userId);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(new { Message = "Edit avatar Successfully" });
            }
            return new BadRequestObjectResult(new { Message = response.Description });                     
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            var response = await _profileService.Get(_userId);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Data);
            }
            return new BadRequestObjectResult(new { Message = response.Description });
        }

        [HttpGet("getProfileWithAnimeList")]
        public IActionResult GetProfileWithAnimeList()
        {
            var response = _profileService.GetProfileWithAnimeList(_userId);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Data);
            }
            return new BadRequestObjectResult(new { Message = response.Description });
        }

        [HttpGet("addAnimeToList/{animeId}")]
        public IActionResult AddAnimeToList([FromRoute] int animeId)
        {
            var response = _profileService.AddAnimeToList(_userId, animeId);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Data);
            }
            return new BadRequestObjectResult(new { Message = response.Description });
        }

        [HttpDelete("deleteAnimeFromList/{animeId}")]
        public IActionResult DeleteAnimeFromList ([FromRoute] int animeId)
        {
            var response = _profileService.DeleteAnimeFromList(animeId, _userId);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Data);
            }
            return new BadRequestObjectResult(new { Message = response.Description });
        }

        [HttpPost("changeUserRating/{id}/{rating?}")]
        public IActionResult ChangeUserRating([FromRoute] int id, [FromRoute] int? rating)
        {
            var response = _profileService.ChangeUserRating(id, rating);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Data);
            }
            return new BadRequestObjectResult(new { Message = response.Description });
        }

        [HttpPost("changeWatchedEpisides/{id}/{episodes?}")]
        public IActionResult ChangeWatchedEpisodes([FromRoute] int id, [FromRoute] int? episodes)
        {
            var response = _profileService.ChangeWatchedEpisodes(id, episodes);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Data);
            }
            return new BadRequestObjectResult(new { Message = response.Description });
        }

        [HttpPost("changeAnimeStatus/{id}/{status}")]
        public IActionResult ChangeAnimeStatus([FromRoute] int id, [FromRoute] string status)
        {
            var response = _profileService.ChangeAnimeStatus(id, status);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Data);
            }
            return new BadRequestObjectResult(new { Message = response.Description });
        }
    }
}
