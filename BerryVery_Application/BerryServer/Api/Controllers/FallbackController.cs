using Microsoft.AspNetCore.Mvc;

namespace BerryServer.WebApi.Controllers
{
    [Route("{*url}")]
    [ApiController]
    public class FallbackController : ControllerBase
    {
        private readonly ILogger<FallbackController> _logger;

        public FallbackController(ILogger<FallbackController> logger)
        {
            this._logger = logger;

            this._logger.LogInformation("FallbackController initialized");
        }

        [AcceptVerbs("GET", "POST", "PUT", "DELETE", "PATCH", "OPTIONS")]
        public IActionResult Default()
        {
            this._logger.LogError("URL Error");

            // 404 Not Found
            return base.NotFound(new
            {
                success = false,
                message = "요청하신 URL을 찾을 수 없습니다."
            });
        }
    }
}
