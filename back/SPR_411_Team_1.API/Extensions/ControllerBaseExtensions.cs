using Microsoft.AspNetCore.Mvc;
using SPR_411_Team_1.BLL.Services;

namespace SPR_411_Team_1.API.Extensions
{
    public static class ControllerBaseExtensions
    {
        public static IActionResult GetResult(this ControllerBase controller, ServiceResponse response)
        {
            if (response.IsSuccess)
            {
                return controller.Ok(response);
            }

            return controller.BadRequest(response);
        }
    }
}
