using Microsoft.AspNetCore.Mvc;
using SPR_411_Team_1.API.Extensions;
using SPR_411_Team_1.BLL.Services;

namespace SPR_411_Team_1.API.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class FilesController : ControllerBase
    {
        private readonly FileService _fileService;

        public FilesController(FileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost("image")]
        public async Task<IActionResult> SaveImageAsync([FromForm] IFormFile image, [FromForm] string destPath)
        {
            var response = await _fileService.SaveImageAsync(image, destPath);
            return this.GetResult(response);
        }
    }
}
