using AnimeList.Domain.RequestModels;
using AnimeList.Domain.RequestModels.SearchAnime;
using AnimeList.Domain.ResponseModel;
using AnimeList.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        public async Task<IActionResult> GetAllAnimeWithGenres([FromQuery] Filter filter)
        {
            var response = await _animeService.GetAll(filter);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Data);
            }
            return new BadRequestObjectResult(new { Message = response.Description });
        }

        [HttpPost("create")]
        public IActionResult Create([FromForm] AnimeRequestModel model)
        {
            var response = _animeService.Create(model);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Data);
            }
            return new BadRequestObjectResult(new { Message = response.Description });
        }

        [HttpPost("get/{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            var response = _animeService.Get(id);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Data);
            }
            return new BadRequestObjectResult(new { Message = response.Description });
        }

        [HttpPost("edit")]
        public IActionResult Edit([FromForm] AnimeRequestModel model)
        {
            var response = _animeService.Edit(model);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Data);
            }
            return new BadRequestObjectResult(new { Message = response.Description });
        }

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
