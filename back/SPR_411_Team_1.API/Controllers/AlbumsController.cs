using Microsoft.AspNetCore.Mvc;
using SPR_411_Team_1.API.Extensions;
using SPR_411_Team_1.BLL.Services;
using SPR_411_Team_1.DAL.Data.Entities;

namespace SPR_411_Team_1.API.Controllers
{
    [ApiController]
    [Route("api/albums")]
    public class AlbumsController : ControllerBase
    {
        private readonly AlbumService _albumService;

        public AlbumsController(AlbumService albumService)
        {
            _albumService = albumService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var response = await _albumService.GetAllAsync();
            return this.GetResult(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var response = await _albumService.GetByIdAsync(id);
            return this.GetResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Album entity)
        {
            var response = await _albumService.CreateAsync(entity);
            return this.GetResult(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] Album entity)
        {
            var response = await _albumService.UpdateAsync(entity);
            return this.GetResult(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var response = await _albumService.DeleteAsync(id);
            return this.GetResult(response);
        }
    }
}
