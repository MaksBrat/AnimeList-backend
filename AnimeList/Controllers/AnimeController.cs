using AnimeList.Domain.RequestModels;
using AnimeList.Domain.RequestModels.EntitiesFilters;
using AnimeList.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AnimeList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimeController : ControllerBase
    {   
        private readonly IAnimeService _animeService;

        public AnimeController(IAnimeService animeService)
        {
            _animeService = animeService;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllAnime([FromQuery] AnimeFilterRequest filterRequest)
        {
            var response = await _animeService.GetAll(filterRequest);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Data);
            }
            return new BadRequestObjectResult(new { Message = response.Description });
        }

        [Authorize]
        [HttpPost("create")]
        public IActionResult Create([FromBody] AnimeRequest model)
        {
            var response = _animeService.Create(model);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Data);
            }
            return new BadRequestObjectResult(new { Message = response.Description });
        }

        [HttpGet("get/{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            var response = _animeService.Get(id);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Data);
            }
            return new BadRequestObjectResult(new { Message = response.Description });
        }

        [Authorize]
        [HttpPost("edit")]
        public IActionResult Edit([FromBody] AnimeRequest model)
        {
            var response = _animeService.Edit(model);
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
            var response = _animeService.Delete(id);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Data);
            }
            return new BadRequestObjectResult(new { Message = response.Description });
        }
    }
}
