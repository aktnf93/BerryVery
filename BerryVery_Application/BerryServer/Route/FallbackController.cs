using Microsoft.AspNetCore.Mvc;

namespace BerryServer.Route
{
    [ApiController]
    public class FallbackController : ControllerBase
    {
        [HttpGet]
        public IActionResult FallbackAction()
        {
            var result = new
            {
                success = false,
                message = "요청하신 URL을 찾을 수 없습니다."
            };
            return base.NotFound(result);
        }
    }
}
