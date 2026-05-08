using Microsoft.AspNetCore.Mvc;
using SPR_411_Team_1.API.Extensions;
using SPR_411_Team_1.BLL.Services;
using SPR_411_Team_1.DAL.Data.Entities;

namespace SPR_411_Team_1.API.Controllers
{
    [ApiController]
    [Route("api/song-genres")]
    public class SongGenresController : ControllerBase
    {
        private readonly SongGenreService _songGenreService;

        public SongGenresController(SongGenreService songGenreService)
        {
            _songGenreService = songGenreService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var response = await _songGenreService.GetAllAsync();
            return this.GetResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] SongGenre entity)
        {
            var response = await _songGenreService.CreateAsync(entity);
            return this.GetResult(response);
        }

        [HttpDelete("{songId}/{genreId}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int songId, [FromRoute] int genreId)
        {
            var response = await _songGenreService.DeleteAsync(songId, genreId);
            return this.GetResult(response);
        }
    }
}
