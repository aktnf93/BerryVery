using BerryServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace BerryServer.Controllers.Api
{
    [Route("api/[Controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private ILogger<UserController> _logger;
        private UserService _service;

        public UserController(ILogger<UserController> logger, UserService service)
        {
            this._logger = logger;
            this._service = service;
        }

        [HttpGet("port")]
        public IActionResult GetPort()
        {
            return base.Ok(1);
        }
    }
}
