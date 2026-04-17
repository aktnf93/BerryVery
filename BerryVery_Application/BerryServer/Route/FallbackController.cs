using Microsoft.AspNetCore.Mvc;

namespace BerryServer.Route
{
    [Route("Fallback")]
    [ApiController]
    public class FallbackController : ControllerBase
    {
        public IActionResult FallbackAction()
        {
            return NotFound(new
            {
                success = false,
                message = "요청하신 URL을 찾을 수 없습니다."
            });
        }
    }
}
