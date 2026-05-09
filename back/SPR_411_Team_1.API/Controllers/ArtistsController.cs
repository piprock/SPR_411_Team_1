using Microsoft.AspNetCore.Mvc;
using SPR_411_Team_1.API.Extensions;
using SPR_411_Team_1.BLL.Services;
using SPR_411_Team_1.DAL.Data.Entities;

namespace SPR_411_Team_1.API.Controllers
{
    [ApiController]
    [Route("api/artists")]
    public class ArtistsController : ControllerBase
    {
        private readonly ArtistService _artistService;

        public ArtistsController(ArtistService artistService)
        {
            _artistService = artistService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var response = await _artistService.GetAllAsync();
            return this.GetResult(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var response = await _artistService.GetByIdAsync(id);
            return this.GetResult(response);
        }

        [HttpGet("by-name/{name}")]
        public async Task<IActionResult> GetByNameAsync([FromRoute] string name)
        {
            var response = await _artistService.GetByNameAsync(name);
            return this.GetResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Artist entity)
        {
            var response = await _artistService.CreateAsync(entity);
            return this.GetResult(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] Artist entity)
        {
            var response = await _artistService.UpdateAsync(entity);
            return this.GetResult(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var response = await _artistService.DeleteAsync(id);
            return this.GetResult(response);
        }
    }
}
