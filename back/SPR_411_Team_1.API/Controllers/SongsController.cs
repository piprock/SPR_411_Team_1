using Microsoft.AspNetCore.Mvc;
using SPR_411_Team_1.API.Extensions;
using SPR_411_Team_1.API.Models;
using SPR_411_Team_1.BLL.Services;

namespace SPR_411_Team_1.API.Controllers
{
    [ApiController]
    [Route("api/songs")]
    public class SongsController : ControllerBase
    {
        private readonly SongService _songService;

        public SongsController(SongService songService)
        {
            _songService = songService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var response = await _songService.GetAllAsync();
            return this.GetResult(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var response = await _songService.GetByIdAsync(id);
            return this.GetResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] SongRequest request)
        {
            var response = await _songService.CreateAsync(request.Song, request.GenreIds);
            return this.GetResult(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] SongRequest request)
        {
            var response = await _songService.UpdateAsync(request.Song, request.GenreIds);
            return this.GetResult(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var response = await _songService.DeleteAsync(id);
            return this.GetResult(response);
        }
    }
}
