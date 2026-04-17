using Microsoft.AspNetCore.Mvc;

namespace BerryServer.Controllers
{
<<<<<<< HEAD:BerryVery_Application/BerryServer/Route/FallbackController.cs
    [Route("Fallback")]
    [ApiController]
    public class FallbackController : ControllerBase
    {
        public IActionResult FallbackAction()
=======
    [Route("{*url}")]
    [ApiController]
    public class FallbackController : ControllerBase
    {
        [HttpGet]
        public IActionResult Default()
>>>>>>> origin/main:BerryVery_Application/BerryServer/Controllers/FallbackController.cs
        {
            return NotFound(new
            {
                success = false,
                message = "요청하신 URL을 찾을 수 없습니다."
            });
        }
    }
}
