using Microsoft.AspNetCore.Mvc;

namespace BerryServer.Controllers
{
    public class MyErrorObjController : ControllerBase
    {
        [HttpGet]
        public IActionResult HandleFallback()
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
